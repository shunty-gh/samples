<?php
$encb64 = "cJ4ZJD3Vkf3Dv5uxrWiTQg==";
$pwd = "123";
$salt = "3";

$enc = base64_decode($encb64);
$decpad = Decrypt($enc, $pwd, $salt);
// Remove the padding
$pad = ord($decpad[($len = strlen($decpad)) - 1]);
$dec = substr($decpad, 0, strlen($decpad) - $pad);

echo "Enc: " . bin2hex($enc) . "\r\n";
echo "Dec: " . $dec . "\r\n";

function Decrypt($ciphertext, $password, $salt)
{
    $key = PBKDF1($password, $salt, 100, 32);
    $iv = PBKDF1($password, $salt, 100, 16);
	
	//echo "Key: " . bin2hex($key) . "\r\n";
	//echo "IV: " . bin2hex($iv) . "\r\n";
	
	// NB: Need 128 not 256 and CBC mode to be compatible
	$result = mcrypt_decrypt(MCRYPT_RIJNDAEL_128, $key, $ciphertext, MCRYPT_MODE_CBC, $iv);
	return $result;
}

function PBKDF1($pass, $salt, $count, $cb)
{
    // This is very approximately the way that the Microsoft version of 
	// PasswordDeriveBytes works.
	
	///
	/// !!!WARNING!!!
	///
	// This is a BAD function!
	// Irrespective of the fact that the use of PBKDF1 is not recommended anyway.
	//
	// This really should be put into a class with a constructor taking the 
	// $pass, $salt and $count.
	// Then there should be a Reset() method to start from scratch each time a new pwd/salt is used.
	// And there should be a GetBytes(int) method to get the required info.
	// But for the sake of simplicity we are assuming the same pwd and salt for each call to 
	// this function. This will not stand up to any scrutiny!
	
	static $base;
	static $extra;
	static $extracount= 0;
	static $hashno;
	static $state = 0;
	
	if ($state == 0)
	{
	  $hashno = 0;
	  $state = 1;
	  
      $key = $pass . $salt;
      $base = sha1($key, true);
      for($i = 2; $i < $count; $i++)
      {
        $base = sha1($base, true);
      }
	}
	
	$result = "";

    // Check if we have any bytes left over from a previous iteration.
	// This is the way MS appears to do it. To me it looks very badly wrong
	// in the line: "$result = substr($extra, $rlen, $rlen);"
    // I'm sure it should be more like "$result = substr($extra, $extracount, $rlen);"
	// Mono have provided what looks like a fixed version at
	// https://github.com/mono/mono/blob/master/mcs/class/corlib/System.Security.Cryptography/PasswordDeriveBytes.cs
	// But I'm no cryptographer so I might be wrong.
	// But this seems to work for low values of $hashno and seems to work
	// with C# implementations.
	
	if ($extracount > 0)
	{
	  $rlen = strlen($extra) - $extracount;
	  if ($rlen >= $cb)
	  {
	    $result = substr($extra, $extracount, $cb);
		if ($rlen > $cb)
		{
		  $extracount += $cb;
		}
		else
		{
		  $extra = null;
		  $extracount = 0;
		}
		return $result;
	  }
	  $result = substr($extra, $rlen, $rlen);
	}
	
	$current = "";
	$clen = 0;
	$remain = $cb - strlen($result);
	while ($remain > $clen)
	{
	  if ($hashno == 0)
	  {
        $current = sha1($base, true);
	  }
	  else if ($hashno < 1000)
	  {
        $n = sprintf("%d", $hashno);
        $tmp = $n . $base;
        $current .= sha1($tmp, true);
      }
	  $hashno++;
	  $clen = strlen($current);	  
	}
	
    // $current now holds at least as many bytes as we need
    $result .= substr($current, 0, $remain);
	// Save any left over bytes
	if ($clen > $remain)
	{
	  $extra = $current;
	  $extracount = $remain;
	}
	
    return $result; 
}

?>