<?php
// Sample created in response to http://stackoverflow.com/q/34306804
// See also the corresponding LinqPad sample

function encrypt($plaintext) {
    $ivsize = mcrypt_get_iv_size(MCRYPT_RIJNDAEL_256, MCRYPT_MODE_CBC);
    $keysize = mcrypt_get_key_size(MCRYPT_RIJNDAEL_256, MCRYPT_MODE_CBC);
    $blocksize = mcrypt_get_block_size(MCRYPT_RIJNDAEL_256, MCRYPT_MODE_CBC);
    //echo "IV Size: " . $ivsize . "\n";
    //echo "Key Size: " . $keysize . "\n";
    //echo "Block Size: " . $blocksize . "\n";

    $masterkey = 'masterKeyOfLength29Characters';
    $td = mcrypt_module_open(MCRYPT_RIJNDAEL_256, '', MCRYPT_MODE_CBC, '');

    //$iv = mcrypt_create_iv(mcrypt_enc_get_iv_size($td), MCRYPT_RAND);
	  $ivbytes = array(72, 163, 99, 62, 219, 111, 163, 114, 15, 47, 65, 99, 231, 108, 110, 87, 72, 163, 99, 62, 219, 111, 163, 114, 15, 47, 65, 99, 231, 108, 110, 87);
    $iv = implode(array_map("chr", $ivbytes));

		//$key = mhash(MHASH_SHA256, $masterkey);
		//echo "Key: " . base64_encode($key) . "\n";
		    
    mcrypt_generic_init($td, $masterkey, $iv);
    //mcrypt_generic_init($td, substr($key, 0, $keysize), $iv);
    //mcrypt_generic_init($td, $key, $iv);
    $crypttext = mcrypt_generic($td, $plaintext);
    mcrypt_generic_deinit($td);
    return base64_encode($iv.$crypttext);
    //return base64_encode($crypttext);
}
//$param = array("key" => "value");
//$enc = encrypt(json_encode($param));
$param = "XPllo world!1234567890123456789";
$enc = encrypt($param);
$encryptedString = rawurlencode($enc);

echo "Input: " . $param . "\n";
//echo "Json data: " . json_encode($param) . "\n";
echo "Base64 encrypted: \n" . $enc . "\n";
//echo "Encrypted: " . $encryptedString . "\n";

?>