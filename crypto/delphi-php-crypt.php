<?php

$enc = 'WRaG/8xlxqqcTAJ5UAk4DA==';
$dec = decrypt_SO($enc, 'test');
echo "Decrypted string: $dec\n";

$src = 'this is a test';
$encr = encrypt_SO($src, 'test');
echo "Encrypted string: $encr\n";

if (($encr == $enc) && ($src == $dec))
{
    echo "Encryption gives correct result";
}
else
{
    echo "Encryption failed";
}
echo "\r\n";


function decrypt_SO($str, $key)
{
    $ivsize = mcrypt_get_iv_size(MCRYPT_DES, MCRYPT_MODE_CBC);
    $keysize = mcrypt_get_key_size(MCRYPT_DES, MCRYPT_MODE_CBC);
    $blocksize = mcrypt_get_block_size(MCRYPT_DES, MCRYPT_MODE_CBC);

    // Need to use the SAME IV as the Delphi function
    $ivbytes = array(72, 163, 99, 62, 219, 111, 163, 114);
    $iv = implode(array_map("chr", $ivbytes));

    $enc = base64_decode($str);
    $k = mhash(MHASH_SHA1, $key);
    $dec = mcrypt_decrypt(MCRYPT_DES, substr($k, 0, $keysize), $enc, MCRYPT_MODE_CBC, $iv);

    $pad = ord($dec[strlen($dec) - 1]);
    return substr($dec, 0, strlen($dec) - $pad);
}

function encrypt_SO($str, $key)
{
    $keysize = mcrypt_get_key_size(MCRYPT_DES, MCRYPT_MODE_CBC);
    $blocksize = mcrypt_get_block_size(MCRYPT_DES, MCRYPT_MODE_CBC);

    // Need to use the SAME IV as the Delphi function
	$ivbytes = array(72, 163, 99, 62, 219, 111, 163, 114);
    $iv = implode(array_map("chr", $ivbytes));

    // Get the key
	$k = mhash(MHASH_SHA1, $key);
	
	// Add the padding
	$padsize = $blocksize - (strlen($str) % $blocksize);
	$str .= str_repeat(chr($padsize), $padsize);
	
	// Encrypt it
    $dec = mcrypt_encrypt(MCRYPT_DES, substr($k, 0, $keysize), $str, MCRYPT_MODE_CBC, $iv);
	// Encode it
    $enc = base64_encode($dec);
 
    return $enc;
}

/*
function decrypt_SO($str, $key)
{
    $ivsize = mcrypt_get_iv_size(MCRYPT_DES, MCRYPT_MODE_CBC);
    $keysize = mcrypt_get_key_size(MCRYPT_DES, MCRYPT_MODE_CBC);
    $blocksize = mcrypt_get_block_size(MCRYPT_DES, MCRYPT_MODE_CBC);

    echo "IV Size: " . $ivsize . "\n";
    echo "Key Size: " . $keysize . "\n";
    echo "Block Size: " . $blocksize . "\n";
    echo "SHA1 key: " . sha1($key) . "\n";
    echo "SHA1 key hex: " . bin2hex(sha1($key)) . "\n";

    //$iv = mcrypt_create_iv($size, MCRYPT_DEV_RANDOM);
    $ivbytes = array(72, 163, 99, 62, 219, 111, 163, 114);
    //$ivbytes = array(0,0,0,0,0,0,0,0);
    $iv = implode(array_map("chr", $ivbytes));

    $enc = base64_decode($str);
    $k = mhash(MHASH_SHA1, $key);
    //$k = substr(sha1($key), 0, $keysize);
    $dec = mcrypt_decrypt(MCRYPT_DES, $k, $enc, MCRYPT_MODE_CBC, $iv);

    echo "Key source: " . bin2hex($key) . "\n";
    echo "Key hash: " . bin2hex(sha1($key)) . "\n";
    echo "Key: " . bin2hex($k) . "\n";
    echo "IV: " . bin2hex($iv) . "\n";
    echo "Dec: " . $dec . "\n";

    $pad = ord($dec[strlen($dec) - 1]);

    return substr($dec, 0, strlen($dec) - $pad);
}

//$enc = 'TW5mbVFhODUyR2FoOTA2WWJIOD0=';
//$enc = 'WRaG/8xlxqoWEpa59JI=';
//$enc = 'WRaG/8xlxqqcTAJ5UAk4DA==';
$enc = 'gT5sKe8SlZxz0s0FTyjETQ==';
$dec = decrypt_SO($enc, 'test');
echo "$dec\n";
*/
?>
