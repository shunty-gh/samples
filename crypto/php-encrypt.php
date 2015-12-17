<?php
echo encrypt("this is a test", "test");

function encrypt($str, $key)
{
    $keysize = mcrypt_get_key_size(MCRYPT_DES, MCRYPT_MODE_CBC);
    $blocksize = mcrypt_get_block_size(MCRYPT_DES, MCRYPT_MODE_CBC);

    // Get fixed IV - oooh BAD BAD BAD!
	$ivbytes = array(72, 163, 99, 62, 219, 111, 163, 114);
    $iv = implode(array_map("chr", $ivbytes));

    // Get the key
	$k = mhash(MHASH_SHA1, $key);
	
	// Add the padding
	$padsize = $blocksize - (strlen($str) % $blocksize);
	$str .= str_repeat(chr($padsize), $padsize);
	echo $str . "\r\n";
	
	// Encrypt it
    $dec = mcrypt_encrypt(MCRYPT_DES, substr($k, 0, $keysize), $str, MCRYPT_MODE_CBC, $iv);
	// Encode it
    $enc = base64_encode($dec);
 
    return $enc;
}
?>
