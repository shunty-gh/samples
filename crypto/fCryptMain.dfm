object frmCryptMain: TfrmCryptMain
  Left = 0
  Top = 0
  BorderIcons = [biSystemMenu, biMinimize]
  Caption = 'Crypto Test'
  ClientHeight = 293
  ClientWidth = 459
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  DesignSize = (
    459
    293)
  PixelsPerInch = 96
  TextHeight = 13
  object lblEncryptSource: TLabel
    Left = 8
    Top = 19
    Width = 73
    Height = 13
    Caption = 'Encrypt Source'
    FocusControl = txtEncryptSource
  end
  object lblEncrypted: TLabel
    Left = 8
    Top = 46
    Width = 49
    Height = 13
    Caption = 'Encrypted'
    FocusControl = txtDecrypted
  end
  object lblDecryptSource: TLabel
    Left = 8
    Top = 139
    Width = 74
    Height = 13
    Caption = 'Decrypt Source'
    FocusControl = txtDecryptSource
  end
  object lblDecrypted: TLabel
    Left = 8
    Top = 166
    Width = 50
    Height = 13
    Caption = 'Decrypted'
    FocusControl = txtDecrypted
  end
  object txtEncryptSource: TEdit
    Left = 113
    Top = 16
    Width = 338
    Height = 21
    Anchors = [akLeft, akTop, akRight]
    TabOrder = 0
    Text = 'txtEncryptSource'
  end
  object txtEncrypted: TEdit
    Left = 113
    Top = 43
    Width = 338
    Height = 21
    TabStop = False
    Anchors = [akLeft, akTop, akRight]
    ReadOnly = True
    TabOrder = 1
    Text = 'txtEncrypted'
  end
  object txtDecryptSource: TEdit
    Left = 113
    Top = 136
    Width = 338
    Height = 21
    Anchors = [akLeft, akTop, akRight]
    TabOrder = 3
    Text = 'txtDecryptSource'
  end
  object txtDecrypted: TEdit
    Left = 113
    Top = 163
    Width = 338
    Height = 21
    TabStop = False
    Anchors = [akLeft, akTop, akRight]
    ReadOnly = True
    TabOrder = 4
    Text = 'txtDecrypted'
  end
  object btnEncrypt: TButton
    Left = 113
    Top = 70
    Width = 104
    Height = 25
    Caption = '&Encrypt'
    TabOrder = 2
    OnClick = DoEncrypt
  end
  object btnDecrypt: TButton
    Left = 113
    Top = 190
    Width = 104
    Height = 25
    Caption = '&Decrypt'
    TabOrder = 5
    OnClick = DoDecrypt
  end
  object btnClose: TButton
    Left = 376
    Top = 262
    Width = 75
    Height = 25
    Anchors = [akRight, akBottom]
    Cancel = True
    Caption = 'Close'
    TabOrder = 6
    OnClick = btnCloseClick
  end
  object btnCopyEncrypted: TButton
    Left = 113
    Top = 101
    Width = 104
    Height = 25
    Caption = 'Copy Encrypted'
    TabOrder = 7
    OnClick = btnCopyEncryptedClick
  end
  object btnHash: TButton
    Left = 223
    Top = 70
    Width = 104
    Height = 25
    Caption = '&Hash'
    TabOrder = 8
    OnClick = DoHash
  end
  object Button1: TButton
    Left = 333
    Top = 70
    Width = 104
    Height = 25
    Caption = '&Encrypt DES'
    TabOrder = 9
    OnClick = DoEncryptDES
  end
  object Button2: TButton
    Left = 333
    Top = 101
    Width = 104
    Height = 25
    Caption = '&Encrypt SO'
    TabOrder = 10
    OnClick = DoEncryptSO
  end
  object Button3: TButton
    Left = 333
    Top = 190
    Width = 104
    Height = 25
    Caption = '&Decrypt DES'
    TabOrder = 11
  end
  object Button4: TButton
    Left = 333
    Top = 221
    Width = 104
    Height = 25
    Caption = '&Decrypt SO'
    TabOrder = 12
    OnClick = DoDecryptSO
  end
  object Button5: TButton
    Left = 223
    Top = 101
    Width = 104
    Height = 25
    Caption = 'HMAC MD5'
    TabOrder = 13
    OnClick = DoHMACMD5
  end
end
