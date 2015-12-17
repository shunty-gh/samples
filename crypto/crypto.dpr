program crypto;

uses
  Forms,
  fCryptMain in 'fCryptMain.pas' {frmCryptMain},
  cryptutils in 'cryptutils.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.MainFormOnTaskbar := True;
  Application.CreateForm(TfrmCryptMain, frmCryptMain);
  Application.Run;
end.
