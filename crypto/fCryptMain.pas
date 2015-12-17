unit fCryptMain;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, IOUtils;

type
  TfrmCryptMain = class(TForm)
    txtEncryptSource: TEdit;
    txtEncrypted: TEdit;
    txtDecryptSource: TEdit;
    txtDecrypted: TEdit;
    btnEncrypt: TButton;
    btnDecrypt: TButton;
    btnClose: TButton;
    lblEncryptSource: TLabel;
    lblEncrypted: TLabel;
    lblDecryptSource: TLabel;
    lblDecrypted: TLabel;
    btnCopyEncrypted: TButton;
    btnHash: TButton;
    Button1: TButton;
    Button2: TButton;
    Button3: TButton;
    Button4: TButton;
    Button5: TButton;
    procedure btnCloseClick(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure DoEncrypt(Sender: TObject);
    procedure DoDecrypt(Sender: TObject);
    procedure btnCopyEncryptedClick(Sender: TObject);
    procedure DoHash(Sender: TObject);
    procedure DoEncryptDES(Sender: TObject);
    procedure DoEncryptSO(Sender: TObject);
    procedure DoDecryptSO(Sender: TObject);
    procedure DoHMACMD5(Sender: TObject);
  private
    procedure ClearFields;
  public
  end;

var
  frmCryptMain: TfrmCryptMain;

implementation

uses
  IdSSLOpenSSL, IdHMAC, IdHMACSHA1, IdHashSHA,
  cryptutils;

{$R *.dfm}

const
  //PASSWORD = 'password';
  PASSWORD = 'test';

procedure TfrmCryptMain.FormCreate(Sender: TObject);
begin
  ClearFields;
end;

procedure TfrmCryptMain.btnCloseClick(Sender: TObject);
begin
  Close;
end;

procedure TfrmCryptMain.btnCopyEncryptedClick(Sender: TObject);
begin
  txtDecryptSource.Text := txtEncrypted.Text;
end;

procedure TfrmCryptMain.ClearFields;
begin
  txtEncryptSource.Text := 'this is a test';
  txtEncrypted.Text := '';
  txtDecryptSource.Text := '';
  txtDecrypted.Text := '';
end;

procedure TfrmCryptMain.DoDecrypt(Sender: TObject);
begin
  txtDecrypted.Text := DecryptString(txtDecryptSource.Text, PASSWORD);
end;

procedure TfrmCryptMain.DoDecryptSO(Sender: TObject);
begin
  txtDecrypted.Text := DecryptStringDES_SO(txtDecryptSource.Text);
end;

procedure TfrmCryptMain.DoEncrypt(Sender: TObject);
begin
  txtEncrypted.Text := EncryptString(txtEncryptSource.Text, PASSWORD);
  if (txtDecryptSource.Text = '') then
      txtDecryptSource.Text := txtEncrypted.Text;
end;

procedure TfrmCryptMain.DoEncryptDES(Sender: TObject);
begin
  txtEncrypted.Text := EncryptStringDES(txtEncryptSource.Text, PASSWORD);
  if (txtDecryptSource.Text = '') then
      txtDecryptSource.Text := txtEncrypted.Text;
end;

procedure TfrmCryptMain.DoEncryptSO(Sender: TObject);
begin
  txtEncrypted.Text := EncryptStringDES_SO;
  if (txtDecryptSource.Text = '') then
      txtDecryptSource.Text := txtEncrypted.Text;
end;

procedure TfrmCryptMain.DoHash(Sender: TObject);
var
  buff: TBytes;
  Hash: TIdHMAC;
  HashValue: TBytes;
  hmacclass: THMACClass;
begin
  // If using anything other than SHA1 then you need to load the OpenSSL library
  // otherwise bad things happen as Indy doesn't have built in support for the
  // other hashes like SHA224, SHA256 etc
  LoadOpenSSLLibrary;

  hmacclass := TIdHMACSHA256;
  SetCurrentDir(ExtractFilePath(ParamStr(0)));
  Hash := hmacclass.Create;
  try
    Hash.Key := TEncoding.ASCII.GetBytes('devaee212345678eedede9');
    buff := TEncoding.UTF8.GetBytes(txtEncryptSource.Text);
    //buff := TFile.ReadAllBytes('menu.xml');
    HashValue := Hash.HashValue(buff);
    // HashValue is an empty array, why?
    //Tag := Length(HashValue);
    //TFile.WriteAllBytes('menu.xml.hash', HashValue);
    txtEncrypted.Text := string(BytesToBase64String(HashValue));
  finally
    FreeAndNil(Hash);
  end;
end;

procedure TfrmCryptMain.DoHMACMD5(Sender: TObject);
var
  buff: TBytes;
begin
  buff := HMACMD5StringUnicode(txtEncryptSource.Text);
  txtEncrypted.Text := BytesToHexString(buff);
end;

end.
