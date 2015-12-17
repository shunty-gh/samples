<?php
function Encrypt($src, $key, $iv)
{
  $block = mcrypt_get_block_size(MCRYPT_RIJNDAEL_128, 'cbc');
  $keysize = mcrypt_get_key_size(MCRYPT_RIJNDAEL_128, 'cbc');
  $ivsize = mcrypt_get_iv_size(MCRYPT_RIJNDAEL_128, 'cbc');
  echo "Block size: " . $block . "\r\n";
  echo "Key size: " . $keysize . "\r\n";
  echo "IV size: " . $ivsize . "\r\n";
  $pad = $block - (strlen($src) % $block);
  $src .= str_repeat(chr($pad), $pad);  

  $enc = mcrypt_encrypt(MCRYPT_RIJNDAEL_128, $key, $src, MCRYPT_MODE_CBC, $iv);
  $r = base64_encode($enc);
  return $r;
}

function Decrypt($src, $key, $iv)
{
  $enc = base64_decode($src);
  $dec = mcrypt_decrypt(MCRYPT_RIJNDAEL_128, $key, $enc, MCRYPT_MODE_CBC, $iv);

  $block = mcrypt_get_block_size(MCRYPT_RIJNDAEL_128, 'cbc');
  $pad = ord($dec[($len = strlen($dec)) - 1]);
  return substr($dec, 0, strlen($dec) - $pad);
}

$plain = "Mary had a little lamb";
//$key1 = "09CB0785F13CD0D557C0940E72E0DCDC86CDC89769044E95DB51A782E7D996FFF3";
//$iv1  = "09CB0785F13CD0D557C0940E72E0DCDC";
$key1 = "09CB0785F13CD0D557C0940E72E0DCDC";
$iv1  = "09CB0785F13CD0D5";

$enc1 = Encrypt($plain, $key1, $iv1);
$dec1 = Decrypt($enc1, $key1, $iv1);

echo "Src: " . $plain . "\n";
echo "Enc: " . $enc1 . "\n";
echo "Dec: " . $dec1 . "\n";
?>