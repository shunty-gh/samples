unit cryptutils;

interface

uses
  SysUtils,
  IdHMAC;

function EncryptString(const ASource: string; const APassword: string): string;
function EncryptStringDES(const ASource: string; const APassword: string): string;
function EncryptStringDES_SO: string;
function DecryptString(const ASource: string; const APassword: string): string;
function DecryptStringDES_SO(ASource: string): string;
function BytesToBase64String(Input: TBytes): AnsiString;
function BytesToHexString(Input: TBytes): string;
function HMACMD5Bytes(Input: TBytes): TBytes;
function HMACMD5StringUnicode(Input: string): TBytes;

type
  THMACClass = class of TIdHMAC;

implementation

uses
  IdGlobal, IdSSLOpenSSL, IdHMACMD5, IdHMACSHA1, IdHashSHA,
  DCPcrypt2, DCPrijndael, DCPdes, DCPsha1, DCPbase64, EncdDecd;

function Base64EncodeBytes(Input: TBytes): TBytes;
var
  ilen: integer;
begin
  ilen := Length(Input);
  SetLength(result, ((ilen + 2) div 3) * 4);
  Base64Encode(@Input[0], @result[0], ilen);
end;

function Base64DecodeBytes(Input: TBytes): TBytes;
var
  ilen, rlen: integer;
begin
  ilen := Length(Input);
  SetLength(result, (ilen div 4) * 3);
  rlen := Base64Decode(@Input[0], @result[0], ilen);
  // Adjust the length of the output buffer according to the number of valid
  // b64 characters
  SetLength(result, rlen);
end;

function BytesToBase64String(Input: TBytes): AnsiString;
var
  b: TBytes;
  s: string;
begin
  b := Base64EncodeBytes(Input);
  s := TEncoding.ASCII.GetString(b);
  result := AnsiString(s);
end;

function BytesToHexString(Input: TBytes): string;
var
  i: integer;
begin
  result := '';
  if Length(Input) > 0 then
  begin
    for i := 0 to Length(Input) - 1 do
    begin
      result := result + IntToHex(Input[i], 2);
    end;
  end;
end;

function GetKeyBytes(APassword: string): TBytes;
var
  hash: TDCP_sha1;
  digest: TBytes;
begin
  hash := TDCP_sha1.Create(nil);
  try
    SetLength(digest, hash.GetHashSize div 8);
    hash.Init;
    hash.UpdateStr(AnsiString(APassword));
    hash.Final(digest[0]);

    //SetLength(result, 16);
    //Move(digest, result, 16);
    result := digest;
  finally
    hash.Free;
  end;
end;

function InternalEncryptString(ACipherClass: TDCP_cipherclass; const ASource: string; const APassword: string): string;
var
  cipher: TDCP_cipher;
  src, enc, b64: TBytes;
begin
  cipher := ACipherClass.Create(nil);
  try
    src := TEncoding.UTF8.GetBytes(ASource);
    SetLength(enc, Length(src));
    cipher.InitStr(AnsiString(APassword), TDCP_sha1);
    if (cipher is TDCP_blockcipher) then
    begin
      TDCP_blockcipher(cipher).EncryptCBC(src[0], enc[0], Length(src));
    end
    else
    begin
      enc := TEncoding.UTF8.GetBytes(cipher.EncryptString(ASource));
    end;

    b64 := Base64EncodeBytes(enc);
    //result := Base64EncodeStr(TEncoding.ASCII.GetString(enc));
    result := TEncoding.UTF8.GetString(b64);
  finally
    cipher.Free;
  end;
end;

function EncryptString(const ASource: string; const APassword: string): string;
begin
  result := InternalEncryptString(TDCP_rijndael, ASource, APassword);
end;

function EncryptStringDES(const ASource: string; const APassword: string): string;
begin
  result := InternalEncryptString(TDCP_des, ASource, APassword);
end;

function DecryptString(const ASource: string; const APassword: string): string;
var
  cipher: TDCP_rijndael;
  src, enc, b64: TBytes;
begin
  cipher := TDCP_rijndael.Create(nil);
  try
    b64 := TEncoding.UTF8.GetBytes(ASource);
    enc := Base64DecodeBytes(b64);

    SetLength(src, Length(enc));
    //key := GetKeyBytes(APassword);
    //cipher.Init(key, 160, nil);
    cipher.InitStr(AnsiString(APassword), TDCP_sha1);
    cipher.DecryptCBC(enc[0], src[0], Length(enc));

    result := TEncoding.UTF8.GetString(src);
  finally
    cipher.Free;
  end;
end;

function EncryptStringDES_SO: string;
var
  des: TDCP_des;
  src, enc: TBytes;
  index, slen, bsize, padsize: integer;
begin
  des:=tdcp_des.Create(nil);
  try
    des.InitStr(AnsiString('test'), tdcp_sha1);

    src := TEncoding.UTF8.GetBytes('this is a test');
    slen := Length(src);
    // Add padding
    bsize := des.BlockSize div 8;
    padsize := bsize - (slen mod bsize);
    Inc(slen, padsize);
    SetLength(src, slen);
    for index := padsize downto 1 do
    begin
      src[slen - index] := padsize;
    end;

    SetLength(enc, slen);
    des.EncryptCBC(src[0], enc[0], slen);
    result := string(EncdDecd.EncodeBase64(@enc[0], Length(enc)));
  finally
    des.Free;
  end;
end;

function DecryptStringDES_SO(ASource: string): string;
var
  des: TDCP_des;
  src, dec: TBytes;
  pad, slen: integer;
begin
  des := TDCP_des.Create(nil);
  try
    des.InitStr(AnsiString('test'), tdcp_sha1);

    src := EncdDecd.DecodeBase64(AnsiString(ASource));
    slen := Length(src);
    SetLength(dec, slen);
    des.DecryptCBC(src[0], dec[0], slen);

    // Remove padding
    pad := dec[slen - 1];
    SetLength(dec, slen - pad);

    result := TEncoding.UTF8.GetString(dec);
  finally
    des.Free;
  end;
end;

function HMACMD5StringUnicode(Input: string): TBytes;
var
  buff: TBytes;
begin
  buff := TEncoding.Unicode.GetBytes(Input);
  result := HMACMD5Bytes(buff);
end;

function HMACMD5Bytes(Input: TBytes): TBytes;
var
  Hash: TIdHMAC;
  HashValue, keybytes: TBytes;
  hmacclass: THMACClass;
const
  key: array[0..15] of byte = ($cd,$06,$ca,$7c,$7e,$10,$c9,$9b,$1d,$33,$b7,$48,$5a,$2e,$d8,$08);
begin
  LoadOpenSSLLibrary;

  hmacclass := TIdHMACMD5;
  SetCurrentDir(ExtractFilePath(ParamStr(0)));
  Hash := hmacclass.Create;
  try
    keybytes := TBytes.Create($cd,$06,$ca,$7c,$7e,$10,$c9,$9b,$1d,$33,$b7,$48,$5a,$2e,$d8,$08);
    Hash.Key := keybytes;
    HashValue := Hash.HashValue(Input);
    result := HashValue;
  finally
    FreeAndNil(Hash);
  end;
end;

end.
