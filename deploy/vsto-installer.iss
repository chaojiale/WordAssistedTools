; =====================================================================
; == make-installer.iss
; == Part of VstoAddinInstaller
; == (https://github.com/bovender/VstoAddinInstaller)
; == (c) 2016-2018 Daniel Kraus <bovender@bovender.de>
; == Published under the Apache License 2.0
; == See http://www.apache.org/licenses
; =====================================================================
; == VERSION INFORMATION

; The script will read the version information from a text file by
; default; the text file must contain two lines:
;   1. The semantic version (e.g. 2.1.8-beta.3)
;   2. The four-number Windows file version (e.g., 2.1.8.3)
; Uncomment the line below to define VERSIONFILE, which will make the
; script read the version information from a text file.
; Note: No sanity check is performed; you must make sure the file does
; contain the two lines with the appropriate information.
; #define VERSIONFILE "VERSION.TXT"

; Alternatively, you can indicate the version number directly in this
; script. The version numbers below are overwritten if VERSIONFILE is
; set.

; The version number. It is suggested to use the semantic versionin
; scheme (http://semver.org), but this is not a must. This version
; information may contain text.
#define SEMANTIC_VERSION "0.4"

; The version in four-number format
#define FOUR_NUMBER_VERSION "0.4.0.0"

; The year(s) of publication (e.g., "2014-2017")
#define PUB_YEARS "2025"

; =====================================================================
; == STATIC INFORMATION ABOUT THE ADD-IN

; Target application. This is used internally by the script
; in order to determine the appropriate registry keys etc.,
; and must be one of 'excel' or 'word'.
#define TARGET_HOST "word"
#if (TARGET_HOST != "excel") && (TARGET_HOST != "word") && (TARGET_HOST != "outlook") && (TARGET_HOST != "powerpoint")
  #error You must choose between "excel", "word", "powerpoint", and "outlook" as target host applications. Others are currently not supported.
#endif

; Specific AppID
; Use InnoSetup's Generate UID command from the Tools menu
; to generate a unique ID. Make sure to have this ID start
; with TWO curly brackets.
; *Never* change this UID after the addin has been deployed,
; lest the users of your add-in will have multiple entries
; in their software list (Add/Remove Software).
#define APP_GUID "{{3B6951E3-92A6-4D16-8F87-A798198A88C0}"
#define ADDIN_NAME "WordAssistedTools"

; The ADDIN_SHORT_NAME is used to generate the installer file
; name and may also be used as a suggestion for installation
; directory during system-wide installations. If it is not defined,
; the value of ADDIN_NAME is used instead. Do not use characters
; that are illegal in file names.
#define ADDIN_SHORT_NAME "WordAssistedTools"

#define COMPANY "le~"
#define DESCRIPTION "A Collection of Word Assisted Tools"
#define HOMEPAGE "https://github.com/chaojiale/WordAssistedTools"
#define HOMEPAGE_SUPPORT "https://github.com/chaojiale/WordAssistedTools"
#define HOMEPAGE_UPDATES "https://github.com/chaojiale/WordAssistedTools"

; SOURCEDIR is the directory that contains the files that
; need to be installed; e.g. 'MyProject\bin\Release\'.
; Include a trailing slash!
#define SOURCEDIR "..\build\Release\"

; VSTOFILE is the file that needs to be written to the registry in
; order to activate the add-in.
; This is usually a file named after the Visual Studio project.
#define VSTOFILE "WordAssistedTools.vsto"

; OUTPUTDIR is the directory where the installer will be saved.
#define OUTPUTDIR "output\"

#define LOGFILE "INST-LOG.TXT"


; =====================================================================
; == ADDITIONAL FILES NEEDED DURING COMPILATION

; SETUPFILESDIR is the directory that contains additional
; files needed to create the installer.
; The files below are all expected to be in this directory.
; The SETUPFILESDIR *must* ende with a backslash, unless it is empty.
#define SETUPFILESDIR ".\"
; #define SETUPFILESDIR "setup-files\"

; License file
; #define LICENSE_FILE "license.rtf"
#define LICENSE_FILE "license.txt"

; Icon that is displayed as a file icon in Windows Explorer
; #define INSTALLER_ICO "icon.ico"

; Large installer banner; the size must be 166x314 px (WxH)
; #define INSTALLER_IMAGE_LARGE "logo-large.bmp"

; Small image to display in the setup wizard; 48x48 px
; #define INSTALLER_IMAGE_SMALL "logo-small.bmp"


; =====================================================================

#if !FileExists(AddBackslash(SOURCEDIR) + VSTOFILE)
  #error Did not find the specified VSTOFILE in SOURCEDIR, please check the spelling -- and make sure you have actually built the project!
#endif


[Setup]
#define DOTNET48URL "https://download.visualstudio.microsoft.com/download/pr/2d6bb6b2-226a-4baa-bdec-798822606ff1/8494001c276a4b96804cde7829c04d7f/ndp48-x86-x64-allos-enu.exe"
#define DOTNET48SHA1 "e322e2e0fb4c86172c38a97dc6c71982134f0570"
#define DOTNET48SIZE "121307088"

#define DOTNETURL DOTNET48URL
#define DOTNETSHA1 DOTNET48SHA1
#define DOTNETSIZE DOTNET48SIZE

#define VSTORURL "http://download.microsoft.com/download/7/A/F/7AFA5695-2B52-44AA-9A2D-FC431C231EDC/vstor_redist.exe"
#define VSTORSHA1 "f6022eb966df7af80f6df5db0d00a0b7a8f516b3"
#define VSTORSIZE "40102072"

; =====================================================================
#ifdef VERSIONFILE
  ; Read the semantic and the installer file version from the VERSION file
  #define FILE_HANDLE FileOpen(VERSIONFILE)
  #define SEMANTIC_VERSION FileRead(FILE_HANDLE)
  #define FOUR_NUMBER_VERSION FileRead(FILE_HANDLE)
  #expr FileClose(FILE_HANDLE)
  #pragma message SEMANTIC_VERSION
#endif

AppId={#APP_GUID}
AppName={#ADDIN_NAME}
AppVersion={#SEMANTIC_VERSION}
AppPublisher={#COMPANY}
AppCopyright={#PUB_YEARS} {#COMPANY}
VersionInfoCompany={#COMPANY}
VersionInfoCopyright={#PUB_YEARS} {#COMPANY}
VersionInfoDescription={#DESCRIPTION}
VersionInfoVersion={#FOUR_NUMBER_VERSION}
VersionInfoTextVersion={#SEMANTIC_VERSION}
VersionInfoProductName={#ADDIN_NAME}
VersionInfoProductVersion={#FOUR_NUMBER_VERSION}
VersionInfoProductTextVersion={#SEMANTIC_VERSION}
AppPublisherURL={#HOMEPAGE}
#ifdef HOMEPAGE_SUPPORT
  AppSupportURL={#HOMEPAGE_SUPPORT}
#else
  AppSupportURL={#HOMEPAGE}
#endif
#ifdef HOMEPAGE_UPDATES
  AppUpdatesURL={#HOMEPAGE_UPDATES}
#else
  AppUpdatesURL={#HOMEPAGE}
#endif
OutputDir={#OUTPUTDIR}

AppendDefaultDirName=false
ArchitecturesAllowed=x86 x64
ArchitecturesInstallIn64BitMode=x64
CloseApplicationsFilter=*.*
CreateAppDir=true
DefaultDialogFontName=Segoe UI
DefaultDirName={code:SuggestInstallDir}
DisableDirPage=false
DisableProgramGroupPage=true
DisableReadyPage=false
LanguageDetectionMethod=locale
SetupLogging=false
TimeStampsInUTC=false
#DEFINE UNINSTALLDIR "{app}\uninstall"
UninstallFilesDir={#UNINSTALLDIR}

; Allow normal users to install the addin into their profile.
; This directive also ensures that the uninstall information is
; stored in the user profile rather than a system folder (which
; would require administrative rights).
PrivilegesRequired=lowest

InternalCompressLevel=normal
SolidCompression=true
OutputBaseFilename={#ADDIN_SHORT_NAME}-setup-{#SEMANTIC_VERSION}

#ifdef LICENSE_FILE
  LicenseFile={#SETUPFILESDIR}{#LICENSE_FILE}
#endif

#ifdef INSTALLER_ICO
  SetupIconFile={#SETUPFILESDIR}{#INSTALLER_ICO}
  UninstallDisplayIcon={#AddBackslash(UNINSTALLDIR)}{#INSTALLER_ICO}
#endif

#ifdef INSTALLER_IMAGE_LARGE
  WizardImageFile={#SETUPFILESDIR}{#INSTALLER_IMAGE_LARGE}
  WizardImageStretch=false
  WizardImageBackColor=clWhite
#endif

#ifdef INSTALLER_IMAGE_SMALL
  WizardSmallImageFile={#SETUPFILESDIR}{#INSTALLER_IMAGE_SMALL}
#endif
	
; Inno Downloader Plugin is required for this
; NB: this directive MUST be located at the end of the [setup] section
#include "InnoDownloadPlugin\idp.iss"

[Files]
Source: {#AddBackslash(SOURCEDIR)}*; Excludes: "EdgeEnvFiles,Logs,*.pdb,*.xml,*.log"; DestDir: {app}; Flags: ignoreversion recursesubdirs

; Copy the installer icon, if defined, to the uninstall files dir
#IFDEF INSTALLER_ICO
  Source: {#AddBackslash(SETUPFILESDIR)}{#INSTALLER_ICO}; DestDir: {#UNINSTALLDIR};
#ENDIF

[Registry]
ValueName: EnableVSTOLocalUNC; ValueData: 1; ValueType: dword; Root: HKLM; Subkey: SOFTWARE\Microsoft\Vsto Runtime Setup\v4; Flags: noerror
Check: IsInstallToWps; ValueName: {#ADDIN_NAME}; ValueData:; ValueType: string; Root: HKCU; Subkey: {code:GetWpsInstallSubkey}; Flags: uninsdeletevalue

; Keys for single-user install (HKCU)
Check: not IsMultiUserInstall; ValueName: Description; ValueData: {#DESCRIPTION}; ValueType: string; Root: HKCU; Subkey: {code:GetRegKey}; Flags: uninsdeletekey
Check: not IsMultiUserInstall; ValueName: FriendlyName; ValueData: {#ADDIN_NAME}; ValueType: string; Root: HKCU; Subkey: {code:GetRegKey}; Flags: uninsdeletekey
Check: not IsMultiUserInstall; ValueName: LoadBehavior; ValueData: 3; ValueType: dword; Root: HKCU; Subkey: {code:GetRegKey}; Flags: uninsdeletekey
Check: not IsMultiUserInstall; ValueName: Warmup; ValueType: none; Root: HKCU; Subkey: {code:GetRegKey}; Flags: deletevalue noerror
Check: not IsMultiUserInstall; ValueName: Manifest; ValueData: file:///{code:ConvertSlash|{app}}/{#VSTOFILE}|vstolocal; ValueType: string; Root: HKCU; Subkey: {code:GetRegKey}; Flags: uninsdeletekey

; Same keys again, this time for multi-user install (HKLM32)
Check: IsMultiUserInstall; ValueName: Description; ValueData: {#DESCRIPTION}; ValueType: string; Root: HKLM32; Subkey: {code:GetRegKey}; Flags: uninsdeletekey
Check: IsMultiUserInstall; ValueName: FriendlyName; ValueData: {#ADDIN_NAME}; ValueType: string; Root: HKLM32; Subkey: {code:GetRegKey}; Flags: uninsdeletekey
Check: IsMultiUserInstall; ValueName: LoadBehavior; ValueData: 3; ValueType: dword; Root: HKLM32; Subkey: {code:GetRegKey}; Flags: uninsdeletekey
Check: IsMultiUserInstall; ValueName: Warmup; ValueType: none; Root: HKLM32; Subkey: {code:GetRegKey}; Flags: deletevalue noerror
Check: IsMultiUserInstall; ValueName: Manifest; ValueData: file:///{code:ConvertSlash|{app}}/{#VSTOFILE}|vstolocal; ValueType: string; Root: HKLM32; Subkey: {code:GetRegKey}; Flags: uninsdeletekey

; Same keys again, this time for multi-user install (HKLM64)
Check: IsMultiUserInstall and IsWin64; ValueName: Description; ValueData: {#DESCRIPTION}; ValueType: string; Root: HKLM64; Subkey: {code:GetRegKey}; Flags: uninsdeletekey
Check: IsMultiUserInstall and IsWin64; ValueName: FriendlyName; ValueData: {#ADDIN_NAME}; ValueType: string; Root: HKLM64; Subkey: {code:GetRegKey}; Flags: uninsdeletekey
Check: IsMultiUserInstall and IsWin64; ValueName: LoadBehavior; ValueData: 3; ValueType: dword; Root: HKLM64; Subkey: {code:GetRegKey}; Flags: uninsdeletekey
Check: IsMultiUserInstall and IsWin64; ValueName: Warmup; ValueType: none; Root: HKLM64; Subkey: {code:GetRegKey}; Flags: deletevalue noerror
Check: IsMultiUserInstall and IsWin64; ValueName: Manifest; ValueData: file:///{code:ConvertSlash|{app}}/{#VSTOFILE}|vstolocal; ValueType: string; Root: HKLM64; Subkey: {code:GetRegKey}; Flags: uninsdeletekey

[Tasks]
; Define any tasks in the custom tasks.iss file.

[Code]
var
  WPSCheckBox: TNewCheckBox;
  OfficeCheckBox: TNewCheckBox;
  PageWhetherInstallToWps: TInputOptionWizardPage;
  PageSingleOrMultiUser: TInputOptionWizardPage;
  PageCannotInstall: TInputOptionWizardPage;
  PageDownloadInfo: TOutputMsgWizardPage;
  PageInstallInfo: TOutputMsgWizardPage;
  prerequisitesChecked: boolean;
  prerequisitesMet: boolean;
  exePath: string;
  IsUpdate: boolean;


{ ============ Constants =============  }

const
  WM_CLOSE = $10;
  MAX_PATH = 250;
  MAX_VERSION = 24; //< highest Office version number to check for.
  //MIN_VSTOR_BUILD = 40305; //< minimum required build of VSTO runtime 2010.
  MIN_VSTOR_BUILD = 50000;

{ ============ helpers =============  }
{
  Converts backslashes to forward slashes.
}
function ConvertSlash(Value: string): string;
begin
  StringChangeEx(Value, '\', '/', True);
  Result := Value;
end;

{
  Checks if a file exists and has a valid Sha1 sum.
}
function IsFileValid(file: string; expectedSha1: string): boolean;
var
  actualSha1: string;
begin
  try
    Log('IsFileValid: Testing:  ' + file);
    Log('IsFileValid: Expected: ' + expectedSha1);
    if FileExists(file) then
    begin
      actualSha1 := GetSHA1OfFile(file);
      Log('IsFileValid: Actual:   ' + actualSha1);
    end
    else
    begin
      Log('IsFileValid: File not found!');
    end;
  finally
    result := actualSha1 = expectedSha1;
    if result then
      Log('IsFileValid: Match')
    else
      Log('IsFileValid: Mismatch');
  end;
end;

{
  Returns the path for the Wow6432Node registry tree if the current operating
  system is 64-bit, i.e., simulates WOW64 redirection.
}
function GetWowNode(): string;
begin
  if IsWin64 then
  begin
    result := 'Wow6432Node\';
  end
  else
  begin
    result := '';
  end;
end;

{
  Returns the add-in registry key for the Office app.
}
function GetRegKey(param: string): string;
// var
//   addinCrumb: string;
begin
  // #ifdef REGKEY
  //   addinCrumb := '{#REGKEY}';
  // #else
  //   //addinCrumb := '{#APP_GUID}';
  //   addinCrumb := '{#ADDIN_NAME}';
  // #endif
  // result := 'Software\Microsoft\Office\{#TARGET_HOST}\Addins\' + addinCrumb;
  result := 'Software\Microsoft\Office\{#TARGET_HOST}\Addins\{#ADDIN_NAME}';
end;

{
  Helper function that evaluates the custom PageSingleOrMultiUser page.
}
function IsMultiUserInstall(): Boolean;
begin
  result := PageSingleOrMultiUser.Values[1];
end;

function IsInstallToWps(): Boolean;
begin
  result := WPSCheckBox.Checked;
end;

function GetWpsInstallSubkey(param: string): string;
begin
  if CompareStr('{#TARGET_HOST}', 'word') = 0 then
  begin
    Result := 'SOFTWARE\kingsoft\Office\WPS\AddinsWL';
  end
  else if CompareStr('{#TARGET_HOST}', 'excel') = 0 then
  begin
    Result := 'SOFTWARE\kingsoft\Office\ET\AddinsWL';
  end
  else if CompareStr('{#TARGET_HOST}', 'powerpoint') = 0 then
  begin
    Result := 'SOFTWARE\kingsoft\Office\WPP\AddinsWL';
  end
  else
    begin
    Result := '';
  end;
end;

{ ============= win32 =============  }

function GetProcessID(hProcess: LongInt): LongInt;
external 'GetProcessId@kernel32.dll stdcall delayload setuponly';

function GetWindowThreadProcessId(hWnd: LongInt; var lpdwProcessId: LongInt): LongInt;
external 'GetWindowThreadProcessId@user32.dll stdcall delayload setuponly';

function OpenProcess(dwDesiredAccess: LongInt; bInheritHandle: LongInt; dwProcessId: LongInt): LongInt;
external 'OpenProcess@kernel32.dll stdcall delayload setuponly';

function CloseHandle(hObject: LongInt): LongInt;
external 'CloseHandle@kernel32.dll stdcall delayload setuponly';

function GetProcessImageFileName(hProcess: longint; lpImageFileName: string; nSize: LongInt): LongInt;
external 'GetProcessImageFileNameA@psapi.dll stdcall delayload setuponly';

function GetLogicalDriveStrings(nBufferLength: LongInt; lpBuffer: string): LongInt;
external 'GetLogicalDriveStringsA@kernel32.dll stdcall delayload setuponly';

function QueryDosDevice(lpDeviceName: string; lpTargetPath: string; ucchMax: LongInt): LongInt;
external 'QueryDosDeviceA@kernel32.dll stdcall delayload setuponly';


{
	Identifies the process that owns hWnd and returns the
	executable path that belongs to it. We need this to be
	able to close Office and automatically restart it after
	setup has completed.
}
function GetProcessExePath(hWnd: LongInt): string;
var
	ProcID: LongInt;
	hProc: LongInt;
	FileName: string;
	FileNameLen: LongInt;
	StrLen: longInt;
	Drives: string;
	DriveName, DeviceName: string;
	iNull: integer;
	CallName: string;
begin
	CallName := 'GetProcessExePath('+IntToStr(hWnd)+'): ';

	{ Identify the process that owns the hWnd Window }
	GetWindowThreadProcessId(hWnd, ProcID);

	if ProcID <> 0 then
	begin
		Log(CallName+'Found process ID');

		{ Get a handle for the process }
		hProc := OpenProcess($400, 0, ProcID);

		if hProc <> 0 then
		begin
			Log(CallName+'Obtained process handle');
			FileNameLen := MAX_PATH;
			FileName := StringOfChar(#0, FileNameLen);
			StrLen := GetProcessImageFileName(hProc, FileName, FileNameLen)

			if StrLen <> 0 then
			begin
				FileName := Copy(FileName, 1, Pos(#0, FileName)-1);

				{ The FileName that we got has an MS-DOS device name in it,
				which we need to resolve now. }
				{ First we obtain a list of all available drives; then
				we iterate through the list, obtain the MS-DOS device name
				for each of the drives and check whether the MS-DOS device
				name occurs in our FileName. If yes, we replace it with the
				drive letter. }

				Drives := StringOfChar(#0, MAX_PATH);
				StrLen := GetLogicalDriveStrings(MAX_PATH, Drives);
				Drives := Copy(Drives, 1, StrLen);

				while Length(Drives)>0 do
				begin
					{ Extract a NULL-terminated substring }
					iNull := Pos(#0, Drives);
					if iNull = 0 then iNull := MAX_PATH;
					DriveName := copy(Drives, 1, iNull-2);

					{ Convert the "C:\" style into a device path }
					DeviceName := StringOfChar(#0, MAX_PATH);
					StrLen := QueryDosDevice(DriveName, DeviceName, MAX_PATH);
					DeviceName := Copy(DeviceName, 1, Pos(#0, DeviceName)-1);

					{ Check if we have found "our" device path;
						if so, replace it with a "C:\" style path }
					if Pos(DeviceName, FileName) = 1 then
					begin
						StringChangeEx(FileName, DeviceName, DriveName, false);
						Log(CallName+'Path: '+FileName);
						Result := FileName;
						Exit { the while loop }
					end;
					Delete(Drives, 1, iNull);
				end;
			end;
			Log(CallName+'Closing handle');
			CloseHandle(hProc);
		end
		else
		begin
			Log(CallName+'*** No handle ***');
		end
	end
	else
	begin
		Log(CallName+'*** Process ID not found ***');
	end;
end;

{ =========  environment ==========}

{
  Checks if a given version of an Office application is installed
}
function IsHostVersionInstalled(version: integer): boolean;
var
  key: string;
  lookup1: boolean;
  lookup2: boolean;
begin
  key := 'Microsoft\Office\' + IntToStr(version) + '.0\{#TARGET_HOST}\InstallRoot';
  lookup1 := RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\' + GetWowNode + key);
  
  {
    If checking for version >= 14.0 ("2010"), which was the first version
    that was produced in both 32-bit and 64-bit, on a 64-bit system we
    also need to check a path without  'Wow6434Node'.
  }
  if IsWin64 and (version >= 14) then
  begin
    lookup2 := RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\' + key);
  end;
  
  result := lookup1 or lookup2;
end;

{
  Checks if only Office 2007 is installed
}
// function IsOnly2007Installed(): boolean;
// var
//   i: integer;
// begin
//   result := IsHostVersionInstalled(12);

//   { Iterate through all }
//   for i := 14 to MAX_VERSION do
//   begin
//     if IsHostVersionInstalled(i) then
//     begin
//       result := false;
//       break;
//     end;
//   end;
// end;

{
  Checks if hotfix KB976477 is installed. This hotfix
  is required to make Office 2007 recognize add-ins in
  the HKLM hive as well.
}
function IsHotfixInstalled(): boolean;
begin
  result := RegKeyExists(HKEY_LOCAL_MACHINE,
    'SOFTWARE\Microsoft\Windows\Current Version\Uninstall\KB976477');
end;

{
  Retrieves the build number of an installed Office version
  in OutBuild. Returns true if the requested Office version
  is installed and false if it is not installed.
}
function GetOfficeBuild(OfficeVersion: integer; var OutBuild: integer): boolean;
var
  key: string;
  value: string;
  build: string;
begin
  key := 'SOFTWARE\' + GetWowNode + 'Microsoft\Office\' +
  IntToStr(OfficeVersion) + '.0\Common\ProductVersion';
  if RegQueryStringValue(HKEY_LOCAL_MACHINE, key, 'LastProduct', value) then
  begin
    {
      Office build numbers always have 4 digits, at least as of Feb. 2015;
      from a string '14.0.1234.5000' simply copy 4 characters from the 5th
      position to get the build number. TODO: Make this future-proof.
    }
    build := Copy(value, 6, 4);
    Log('GetOfficeBuild: Found ProductVersion "' + value + '" for queried Office version '
      + IntToStr(OfficeVersion) + ', extracted build number ' + build);
    OutBuild := StrToInt(build);
    result := true;
  end
  else
  begin
    Log('GetOfficeBuild: Did not find LastProduct key for Office version ' +
      IntToStr(OfficeVersion) + '.0.');
  end
end;

{
  Asserts if Office 2007 is installed. Does not check whether other Office
  versions are concurrently installed.
}
// function IsOffice2007Installed(): boolean;
// begin
//   result := IsHostVersionInstalled(12);
//   if result then Log('IsOffice2007Installed: Detected Office 2007.');
// end;


{
  Asserts if Office 2010 is installed.
}
// function IsOffice2010Installed(): boolean;
// begin
//   result := IsHostVersionInstalled(14);
//   if result then Log('IsOffice2010Installed: Detected Office 2010.');
// end;

function IsOffice2016Installed(): boolean;
begin
  result := IsHostVersionInstalled(16);
  if result then Log('IsOffice2016Installed: Detected Office 2016.');
end;

{
  Asserts if Office 2010 without service pack is installed.
  For build number, see http://support.microsoft.com/kb/2121559/en-us
}
// function IsOffice2010NoSpInstalled(): boolean;
// var
//   build: integer;
// begin
//   if GetOfficeBuild(14, build) then
//   begin
//     result := build = 4763; { 4763 is the original Office 2007 build }
//     if result then
//       Log('IsOffice2010NoSpInstalled: Detected Office 2010 without service pack (v. 14.0, build 4763)')
//     else
//     begin
//       Log('IsOffice2010NoSpInstalled: Detected Office 2010, apparently with some service pack (build ' +
//         IntToStr(build) + ').');
//     end
//   end;
// end;


{
  Determines whether or not a system-wide installation
  is possible. This depends on whether the current user
  is an administrator, and whether the hotfix KB976477
  is present on the system if Office 2007 is the only version
  of Office that is present (without that hotfix, Office
  2007 does not load add-ins that are registered in the
  HKLM hive).
}
function CanInstallSystemWide(): boolean;
begin
  //if IsAdminLoggedOn then
  if IsAdmin then
  begin
    // if IsOnly2007Installed then
    // begin
    //   result := IsHotfixInstalled;
    //   if result then
    //     Log('CanInstallSystemWide: Only Office 2007 found, hotfix installed, can install system-wide.')
    //   else
    //     Log('CanInstallSystemWide: Only Office 2007 found but hotfix not installed, cannot install system-wide.')
    // end
    // else
    // begin
      Log('CanInstallSystemWide: User is admin, can install system-wide.')
      result := true;
    // end;
  end
  else
  begin
    Log('CanInstallSystemWide: User is not admin, cannot install system-wide.')
    result := false;
  end;
end;

{ ============ runtimes ============= }
{
  Checks if the VSTO runtime is installed.
  See: http://xltoolbox.sf.net/blog/2015/01/net-vsto-add-ins-getting-prerequisites-right
  HKLM\SOFTWARE\Microsoft\VSTO Runtime Setup\v4R (32-bit)
  HKLM\SOFTWARE\Wow6432Node\Microsoft\VSTO Runtime Setup\v4R (64-bit)
  The 'R' suffix need not be present.
}
function IsVstorInstalled(): boolean;
var
  vstorPath: string;
begin
  vstorPath := 'SOFTWARE\' + GetWowNode + 'Microsoft\VSTO Runtime Setup\v4';
  result := RegKeyExists(HKEY_LOCAL_MACHINE, vstorPath) or
            RegKeyExists(HKEY_LOCAL_MACHINE, vstorPath + 'R');
  if result then
    Log('IsVstorInstalled: VSTO Runtime is installed')
  else
    Log('IsVstorInstalled: VSTO Runtime is not installed');
end;

{
  Extracts the build number from the VSTO runtime version string
  that is stored in the registry.
}
function GetVstorBuild(): integer;
var
  vstorPath: string;
  version: string;
  build: string;
begin
  vstorPath := 'SOFTWARE\' + GetWowNode + 'Microsoft\VSTO Runtime Setup\';
  Log('GetVstorBuild: ' + vstorPath);
  if not RegQueryStringValue(HKEY_LOCAL_MACHINE, vstorPath + 'v4R', 'Version', version) then
  begin
    { Check again without the R suffix. }
    Log('GetVstorBuild: v4R key not found, attempting v4 key');
    if not RegQueryStringValue(HKEY_LOCAL_MACHINE, vstorPath + 'v4', 'Version', version) then
    begin
      Log('GetVstorBuild: v4 key not found either!');
    end
  end;
  if Length(version) > 0 then
  begin
    Log('GetVstorBuild: Version:   ' + version);
    build := Copy(version, 6, 5);
    Log('GetVstorBuild: Build str: ' + build);
    result := StrToIntDef(build, 0);
    Log('GetVstorBuild: Build:     ' + IntToStr(result));
  end
  else
  begin
    Log('GetVstorBuild: Runtime not detected, returning build number 0');
    result := 0;
  end;
end;

{
  Checks if the .NET 4.0 (or 4.5) runtime is installed.
  See https://msdn.microsoft.com/en-us/library/hh925568
  https://learn.microsoft.com/zh-cn/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed
}
function IsNetInstalled(): boolean;
var
  versionId: Cardinal;
begin
  result:=false;
  RegQueryDWordValue(HKEY_LOCAL_MACHINE, 'SOFTWARE\' + GetWowNode + 'Microsoft\NET Framework Setup\NDP\v4\Client', 'Release', versionId);
  if integer(versionId) > 520000 then //min:528040
  begin
    // 读取值成功
    result:=true;
  end;
end;

{
  Indicates whether or not the .NET runtime needs to be installed;
  this takes into account whether the installer runs with the
  /UPDATE switch or not.
}
function NeedToInstallDotNetRuntime(): boolean;
begin
  result := not IsNetInstalled and not IsUpdate;
end;

{
  Asserts if the VSTO runtime for .NET 4.0 redistributable needs to be
  downloaded and installed.
  If Office 2010 SP 1 or newer is installed on the system, the VSTOR runtime
  will be automagically configured as long as the .NET 4.0 runtime is present.
  Office 2007 and Office 2010 without service pack need the VSTO runtime
  redistributable. For details, see:
  http://xltoolbox.sf.net/blog/2015/01/net-vsto-add-ins-getting-prerequisites-right
}
function NeedToInstallVstor(): boolean;
begin
  Log('NeedToInstallVstor: Minimum required VSTOR 2010 build: ' + IntToStr(MIN_VSTOR_BUILD));
  if IsOffice2016Installed then //IsOffice2007Installed or
  begin
    result := false;
  end
  else
  begin
    result := (GetVstorBuild < MIN_VSTOR_BUILD) and not IsUpdate;
  end;

  if result then
    Log('NeedToInstallVstor: Need to install VSTO runtime')
  else
    Log('NeedToInstallVstor: No need to install VSTO runtime');

end;

{
  Checks if all required prerequisites are met, i.e. if the necessary
  runtimes are installed on the system
}
function PrerequisitesAreMet(): boolean;
begin
  { Cache check result to avoid multiple registry lookups and log messages }
  if not prerequisitesChecked then
  begin
    prerequisitesMet := (not NeedToInstallDotNetRuntime) and (not NeedToInstallVstor);
    prerequisitesChecked := true;
  end;
  result := prerequisitesMet;
end;

{
  Returns the path to the downloaded VSTO runtime installer.
}
function GetVstorInstallerPath(): string;
begin
  result := ExpandConstant('{%temp}\vstor_redist.exe');
end;

{
  Returns the path to the downloaded .NET runtime installer.
}
function GetNetInstallerPath(): string;
begin
  result := ExpandConstant('{%temp}\ndp48-x86-x64-allos-enu.exe');
end;

{
  Checks if the VSTO runtime redistributable setup file has already been
  downloaded by comparing SHA1 checksums.
}
function IsVstorDownloaded(): boolean;
begin
  result := IsFileValid(GetVstorInstallerPath, '{#VSTORSHA1}');
end;

{
  Checks if the .NET runtime setup file has already been
  downloaded by comparing SHA1 checksums.
}
function IsNetDownloaded(): boolean;
begin
  result := IsFileValid(GetNetInstallerPath, '{#DOTNETSHA1}');
end;

{
  Determines if the VSTO runtime needs to be downloaded.
  This is not the case it the runtime is already installed,
  or if there is a file with a valid Sha1 sum.
}
function NeedToDownloadVstor: boolean;
begin
  result := NeedToInstallVstor and not IsVstorDownloaded;
end;

{
  Determines if the VSTO runtime needs to be downloaded.
  This is not the case it the runtime is already installed,
  or if there is a file with a valid Sha1 sum.
}
function NeedToDownloadNet: boolean;
begin
  result := NeedToInstallDotNetRuntime and not IsNetDownloaded;
end;

function ExecuteNetSetup(): boolean;
var
  exitCode: integer;
begin
  result := true;
  if NeedToInstallDotNetRuntime then
  begin
    if IsNetDownloaded then
    begin
      Log('NeedToInstallDotNetRuntime: Valid .NET runtime download found, installing.');
      Exec(GetNetInstallerPath, '/norestart',
        '', SW_SHOW, ewWaitUntilTerminated, exitCode);
      BringToFrontAndRestore;
      if not IsNetInstalled then
      begin
        Log('NeedToInstallDotNetRuntime: .NET runtime still not installed, warning user.');
        MsgBox(CustomMessage('StillNotInstalled'), mbInformation, MB_OK);
        result := True;
      end;
    end
    else
    begin
      Log('NeedToInstallDotNetRuntime: No or invalid .NET runtime download found, warning user.');
      MsgBox(CustomMessage('DownloadNotValidated'), mbInformation, MB_OK);
      result := True;
    end;
  end; { NeedToInstallDotNetRuntime }
end;

function ExecuteVstorSetup(): boolean;
var
  exitCode: integer;
begin
  result := true;
  if NeedToInstallVstor then
  begin
    if IsVstorDownloaded then
    begin
      Log('ExecuteVstorSetup: Valid VSTO runtime download found, installing.');
      Exec(GetVstorInstallerPath, '/norestart', '', SW_SHOW,
        ewWaitUntilTerminated, exitCode);
      BringToFrontAndRestore;
      if not IsVstorInstalled then
      begin
        Log('ExecuteVstorSetup: VSTO runtime still not installed, warning user.');
        MsgBox(CustomMessage('StillNotInstalled'), mbInformation, MB_OK);
        result := True;
      end;
    end
    else
    begin
      Log('ExecuteVstorSetup: No or invalid VSTO runtime download found, warning user.');
      MsgBox(CustomMessage('DownloadNotValidated'), mbInformation, MB_OK);
      result := True;
    end;
  end; { not IsVstorInstalled }
end;

{ ============ wizard-pages ============= }
procedure InstallToApplicationPage();
begin
  // Create the page
  PageWhetherInstallToWps := CreateInputOptionPage(wpLicense,
    CustomMessage('InstallToApp'), CustomMessage('InstallToAppSubcaption'),
    CustomMessage('InstallToAppDesc')+Uppercase('{#TARGET_HOST}'), True, False);

  // Create the Office checkbox
  OfficeCheckBox := TNewCheckBox.Create(PageWhetherInstallToWps);
  OfficeCheckBox.Left := ScaleX(0);
  OfficeCheckBox.Top := ScaleY(20);
  OfficeCheckBox.Width := PageWhetherInstallToWps.SurfaceWidth;
  OfficeCheckBox.Height := ScaleY(20);
  OfficeCheckBox.Caption := 'Microsoft Office';
  OfficeCheckBox.Checked := True;
  OfficeCheckBox.Enabled := False;
  OfficeCheckBox.TabOrder := 0;
  OfficeCheckBox.Parent := PageWhetherInstallToWps.Surface;

  // Create the WPS checkbox
  WPSCheckBox := TNewCheckBox.Create(PageWhetherInstallToWps);
  WPSCheckBox.Left := ScaleX(0);
  WPSCheckBox.Top := ScaleY(40);
  WPSCheckBox.Width := PageWhetherInstallToWps.SurfaceWidth;
  WPSCheckBox.Height := ScaleY(20);
  WPSCheckBox.Caption := 'WPS';
  WPSCheckBox.Checked := False;
  WPSCheckBox.TabOrder := 1;
  WPSCheckBox.Parent := PageWhetherInstallToWps.Surface;
  // Add the page to the wizard
end;

procedure CreateSingleOrAllUserPage();
begin
  PageSingleOrMultiUser := CreateInputOptionPage(wpLicense,
    CustomMessage('SingleOrMulti'), CustomMessage('SingleOrMultiSubcaption'),
    CustomMessage('SingleOrMultiDesc'), True, False);
  PageSingleOrMultiUser.Add(CustomMessage('SingleOrMultiSingle'));
  PageSingleOrMultiUser.Add(CustomMessage('SingleOrMultiAll'));
  if CanInstallSystemWide then
  begin
    PageSingleOrMultiUser.Values[1] := True;
  end
  else
  begin
      PageSingleOrMultiUser.Values[0] := True;
  end;
end;

procedure CreateCannotInstallPage();
begin
  PageCannotInstall := CreateInputOptionPage(wpWelcome,
    CustomMessage('CannotInstallCaption'),
    CustomMessage('CannotInstallDesc'),
    CustomMessage('CannotInstallMsg'), True, False);
  PageCannotInstall.Add(CustomMessage('CannotInstallCont'));
  PageCannotInstall.Add(CustomMessage('CannotInstallAbort'));
  PageCannotInstall.Values[1] := True;
end;

procedure CreateDownloadInfoPage();
var
  #IF DEFINED UNICODE
    bytes: Int64;
  #ELSE
    bytes: DWord;
  #ENDIF
  mib: Single;
  size: String;
begin
  if idpGetFilesSize(bytes) then
  begin
    mib := bytes / 1048576;
    size := Format('%.1f', [ mib ]);
  end
  else
  begin
    size := '[?]'
  end;
  PageDownloadInfo := CreateOutputMsgPage(PageSingleOrMultiUser.Id,
    CustomMessage('RequiredCaption'),
    CustomMessage('RequiredDesc'),
    Format(CustomMessage('RequiredMsg'), [idpFilesCount, size]));
end;

procedure CreateInstallInfoPage();
begin
  PageInstallInfo := CreateOutputMsgPage(PageDownloadInfo.Id,
    CustomMessage('InstallCaption'),
    CustomMessage('InstallDesc'),
    CustomMessage('InstallMsg'));
end;

{ ============ detect-running-app ============= }
{
  Returns the class name of the main window of the hosting Office application.
  See http://users.skynet.be/am044448/Programmeren/VBA/vba_class_names.htm
}
function OfficeWindowName(): string;
begin
#if TARGET_HOST == "excel"
  result := 'xlmain';
#elif TARGET_HOST == "word"
  result := 'opusapp';
#elif TARGET_HOST == "powerpoint"
  { TODO: Deal with different PowerPoint versions; PP10FrameClass is for
    PowerPoint XP only... }
  result := 'PP10FrameClass';
#elif TARGET_HOST == "access"
  result := 'omain';
#elif TARGET_HOST == "outlook"
  result := 'rctrl_renwnd32';
#else
  #error Cannot handle this TARGET_HOST value in OfficeWindowName()
#endif
end;

{
  Detect if a given Office application is running and offers
  to close it, or abort the installation.
  windowName: name of the application's main window (e.g. 'XLMAIN')
  Returns true if the app was either not running or has been closed.
  Returns false if the user aborted the installation.
}
function CloseAppInteractively(): boolean;
var
  hWnd: LongInt;
  bCancel: boolean;
begin
  Log('CloseOfficeAppInteractively(''' + OfficeWindowName() + ''')');
  hWnd := FindWindowByClassName(OfficeWindowName());

  {
    If Office is running, hWnd is different from 0.
  }
  while (hWnd <> 0) and not bCancel do
  begin
    exePath := GetProcessExePath(hWnd);
    if SuppressibleMsgBox(CustomMessage('OfficeIsRunning'), 
        mbConfirmation, MB_OKCANCEL, IDOK) <> IDOK then
    begin
      Log('App running - user aborted setup.');
      bCancel := true;
    end
    else
    begin
      Log('App running - attempting to close...');
      { ExcelExePath := GetProcessExePath(Hwnd); }
      SendMessage(hWnd, WM_CLOSE, 0, 0); { WM_CLOSE: $10 }
      Sleep(1000);
      hWnd := FindWindowByClassName(OfficeWindowName());
    end;
  end;

  Result := (hWnd = 0);
end;

{
  Close a given Office application if it is running.
  windowName: name of the application's main window (e.g. 'XLMAIN')
  Returns the path of the executable that was closed so it
  can later be restarted.
  Returns an empty string if the app is not running.
}
function CloseAppNoninteractively(): string;
var
  exePath: string;
  hWnd: LongInt;
begin
  Log('CloseOfficeAppInteractively(''' + OfficeWindowName() + ''')');
  hWnd := FindWindowByClassName(OfficeWindowName());
  exePath := '';
  if hWnd <> 0 then
  begin
    exePath := GetProcessExePath(hWnd);
    Log('Sending WM_CLOSE...');
    SendMessage(hWnd, WM_CLOSE, 0, 0); { WM_CLOSE: $10 }

    {
      After sending the WM_CLOSE message, we need to wait
      a moment to allow the app to shut down; if we did not
      wait, the Setup program would abort if started with /SP- /SILENT.
    }
    Sleep(1000);
  end;
  Result := exePath;
end;

{ ================== other ===============}

{
  Returns true if running on a zero client. The algorithm has only been
  tested for VMware Horizon/Teradici clients.
}
function IsZeroClient(): boolean;
var
  protocol: string;
begin
  if RegQueryStringValue(HKEY_CURRENT_USER, 'Volatile Environment',
    'ViewClient_Protocol', protocol) then
  begin
    Log('IsZeroClient: ViewClient_Protocol: ' + protocol)
    result := Uppercase(protocol) = 'PCOIP';
    if result then
      Log('IsZeroClient: Recognized as zero client')
    else
      Log('IsZeroClient: Not recognized as a zero client')
  end;
end;

{
  Returns true if the target directory chooser should be shown or
  not: This is the case if running on a zero client, or if the
  current user is an administrator.
}
function ShouldShowDirPage(): boolean;
begin
  //result := IsAdminLoggedOn; // or IsZeroClient;
  //result := IsAdmin;
  result := true;
end;


function IsWindowsVersionOrNewer(Major, Minor: Integer): Boolean;
var
  Version: TWindowsVersion;
begin
  GetWindowsVersionEx(Version);
  Result :=
    (Version.Major > Major) or
    ((Version.Major = Major) and (Version.Minor >= Minor));
end;

function IsWindowsXPOrNewer: Boolean;
begin
  Result := IsWindowsVersionOrNewer(5, 1);
end;

function IsWindowsVistaOrNewer: Boolean;
begin
  Result := IsWindowsVersionOrNewer(6, 0);
end;

function IsWindows7OrNewer: Boolean;
begin
  Result := IsWindowsVersionOrNewer(6, 1);
end;

function IsWindows8OrNewer: Boolean;
begin
  Result := IsWindowsVersionOrNewer(6, 2);
end;

function IsWindows10OrNewer: Boolean;
begin
  Result := IsWindowsVersionOrNewer(10, 0);
end;

// Windows 11 has the same major.minor as Windows 10.
// So it has to be distinguished by the Build.
// The IsWindows10OrNewer condition is actually redundant.
// Once we have to test for Windows 11 using the build number, we could actually
// unify and simplify all the tests above to use the build numbers only too.
function IsWindows11OrNewer: Boolean;
var
  Version: TWindowsVersion;
begin
  GetWindowsVersionEx(Version);
  Result := IsWindows10OrNewer and (Version.Build >= 22000);
end;

function InitializeSetup(): boolean;
var
  i: integer;
begin
  if not IsWindows10OrNewer then
  begin 
    MsgBox(CustomMessage('MustUseWindows10OrNewer'), mbInformation, MB_OK);
  end;  

  for i := 1 to ParamCount do
  begin
    if uppercase(ParamStr(i)) = '/UPDATE' then
    begin
      Log('InitializeSetup: /UPDATE switch found');
      IsUpdate := true;
      exePath := CloseAppNoninteractively();
      result := true;
    end
  end;

  if not IsUpdate then
  begin
    result := CloseAppInteractively();
  end;
end;

procedure InitializeWizard();
begin
  InstallToApplicationPage;
  CreateSingleOrAllUserPage;
  if not PrerequisitesAreMet then
  begin
    Log('InitializeWizard: Not all prerequisites are met...');
    CreateCannotInstallPage;
    if NeedToDownloadNet then
    begin
      Log('InitializeWizard: Mark {#DOTNETURL} for download.');
      idpAddFileSize('{#DOTNETURL}', GetNetInstallerPath, {#DOTNETSIZE});
    end;
    if NeedToDownloadVstor then
    begin
      Log('InitializeWizard: Mark {#VSTORURL} for download.');
      idpAddFileSize('{#VSTORURL}', GetVstorInstallerPath, {#VSTORSIZE});
    end;
    CreateDownloadInfoPage;
    CreateInstallInfoPage;
    idpDownloadAfter(PageDownloadInfo.Id);
  end;
end;

function NextButtonClick(CurPageID: Integer): Boolean;
begin
  Log('NextButtonClick: CurPageID = ' + IntToStr(CurPageID));
  result := True;
  if not WizardSilent then
  begin
    {
    if CurPageID = PageDevelopmentInfo.Id then
    begin
      if PageDevelopmentInfo.Values[0] = False then
      begin
        Log('Requesting user to acknowledge use of a developmental version.');
        MsgBox(CustomMessage('DevVerMsgBox'), mbInformation, MB_OK);
        result := False;
      end;
    end;
    }
  end;

  if not PrerequisitesAreMet then
  begin
    {
      Abort the installation if any of the runtimes are missing, the user
      is not an administrator, and requested to abort the installation.
    }
    if CurPageID = PageCannotInstall.ID then
    begin
      if PageCannotInstall.Values[1] = true then
      begin
        Log('NextButtonClick: Non-admin user cannot install, aborting.');
        WizardForm.Close;
        result := False;
      end
      else
      begin
        Log('NextButtonClick: Non-admin user continues although not all required runtimes are installed.');
      end;
    end;

    if CurPageID = PageInstallInfo.ID then
    begin
      { Return true if installation succeeds (or no installation required) }
      result := ExecuteNetSetup and ExecuteVstorSetup;
    end;
  end; { not PrerequisitesAreMet }
end;

procedure CurPageChanged(CurPageID: Integer);
begin
  if CurPageID = wpReady then
  begin
    Wizardform.ReadyMemo.Lines.Add(''); { Empty string }
    Wizardform.ReadyMemo.Lines.Add(CustomMessage('InstallFinalMsg') + CustomMessage('Colon'));
    Wizardform.ReadyMemo.Lines.Add('      Microsoft Office');
    if WPSCheckBox.Checked then
    begin
      Wizardform.ReadyMemo.Lines.Add('      WPS');
    end;
  end;
end;

{
  Skips the folder selection, single/multi user, and ready pages for
  normal users without power privileges.
  This function also takes care of dynamically determining what wizard
  pages to install, depending on the current system setup and whether
  the current user is an administrator.
}
function ShouldSkipPage(PageID: Integer): Boolean;
begin
  result := False;

  if not PrerequisitesAreMet then
  begin
    {
      The PageDownloadCannotInstall will only have been initialized if
      PrerequisitesAreMet returned false.
    }
    if PageID = PageCannotInstall.ID then
    begin
      { Skip the warning if the user is an admin. }
      //result := IsAdminLoggedOn
      result := IsAdmin
      if not result then
      begin
        Log('ShouldSkipPage: Warning user that required runtimes cannot be installed due to missing privileges');
      end;
    end;

    if PageID = PageDownloadInfo.ID then
    begin
      { Skip page informing about downloads if no files need to be downloaded. }
      result := idpFilesCount = 0;
    end;

    if PageID = IDPForm.Page.ID then
    begin
      { Skip downloader plugin if there are no files to download. }
      result := idpFilesCount = 0;
      if not result then
      begin
        Log('ShouldSkipPage: Beginning download of ' + IntToStr(idpFilesCount) + ' file(s).');
      end;
    end;
  end; { not PrerequisitesAreMet }

  if PageID = PageSingleOrMultiUser.ID then
  begin
    if CanInstallSystemWide then
    begin
      Log('ShouldSkipPage: Do not skip multi-user page, offer installation for all users.');
      result := False;
    end
    else
    begin
      Log('ShouldSkipPage: Skip multi-user page, offer single-user installation only.');
      result := True;
    end;
  end;

  if (PageID = wpSelectDir) or (PageID = wpReady) then
  begin
    {
      Do not show the pages to select the target directory, and the ready
      page if the user is not an admin.
    }
    result := not ShouldShowDirPage;
    if result then
      Log('ShouldSkipPage: Skipping target directory query.')
    else
      Log('ShouldSkipPage: Showing target directory query.')
  end
end;

{
  Suggest an initial target directory depending on whether
  the installer is run with admin privileges.
}
function SuggestInstallDir(Param: string): string;
var
  dir: string;
begin
  if CanInstallSystemWide then
  begin
    Log('SuggestInstallDir: Can install system-wide, suggesting Programs folder');
    //dir := ExpandConstant('{pf}');
    dir := 'D:';
  end
  else
  begin
    // if IsZeroClient then
    // begin
    //   Log('SuggestInstallDir: Looks like zero client, suggesting user docs folder');
    //   dir := ExpandConstant('{userdocs}')
    // end
    // else
    // begin
      Log('SuggestInstallDir: Suggesting user profile folder');
      dir := ExpandConstant('{userappdata}')
    // end
  end;
  result := AddBackslash(dir) + '{#ADDIN_SHORT_NAME}';
  Log('SuggestInstallDir: ' + result);
end;



procedure DeinitializeSetup();
var
  e: Integer;
begin
  if Length(exePath) > 0 then
  begin
    Log('DeinitializeSetup: Restarting Office host...');
    Log(exePath);
    Exec(exePath, '', '', SW_SHOW, ewNoWait, e);
  end;

#ifdef LOGFILE
  {
    Copy the log file to the installation
  }
  try
    Log('DeinitializeSetup: Copying log file to installation folder');
    //FileCopy(
    //  ExpandConstant('{log}'),
    //  AddBackslash(ExpandConstant('{app}'))+'{#LOGFILE}', false);
  except
    Log('DeinitializeSetup: Failed to copy log file');
  end
#endif
end;

// Function to check if Node.js is installed
function IsNodeInstalled(): Boolean;
var
  NodePath: String;
begin
  Result := False;
  if RegQueryStringValue(HKLM, 'SOFTWARE\Node.js', 'InstallPath', NodePath) then
  begin
    Result := NodePath <> '';
  end;
end;

[Languages]
Name: en; MessagesFile: compiler:Default.isl; 
Name: cn; MessagesFile: compiler:Languages\ChineseSimplified.isl;

[CustomMessages]
en.Colon=:
cn.Colon=：

en.Yes=Yes
cn.Yes=是

en.No=No
cn.No=否

en.MustUseWindows10OrNewer=This installer requires Windows 10 or newer. Some features may not work on other systems. Proceed with caution.
cn.MustUseWindows10OrNewer=此安装程序需要Windows 10或更高版本，其他系统下部分功能模块可能无法使用。请谨慎操作。

en.OfficeIsRunning=Your Office application must be closed in order to continue installation. If you click 'OK', the application will be shut down.
cn.OfficeIsRunning=您的Office应用程序必须关闭才能继续安装。如果您单击“确定”，安装程序将向应用程序发送一个关闭指令，您也可以手动将应用程序关闭后点击“确定”以继续安装。

en.SingleOrMulti=Single-user or system-wide install
cn.SingleOrMulti=单用户或所有用户安装

en.SingleOrMultiSubcaption=Install for the current user only or for all users
cn.SingleOrMultiSubcaption=仅为当前用户安装还是为所有用户安装

en.SingleOrMultiDesc=Please indicate the scope of this installation:
cn.SingleOrMultiDesc=请选择安装范围：

en.SingleOrMultiSingle=Single user (only for me)
cn.SingleOrMultiSingle=单用户（仅为我）

en.SingleOrMultiAll=All users (system-wide)
cn.SingleOrMultiAll=所有用户（系统范围）

en.Office2007Required=This add-in requires Office 2007 or later. Setup will now terminate.
cn.Office2007Required=此附加组件需要Office 2016或更高版本。安装程序将立即终止。

en.InstallToApp=Install to application
cn.InstallToApp=安装到应用程序

en.InstallToAppSubcaption=Choose the applications to install to
cn.InstallToAppSubcaption=选择要安装到的应用程序

en.InstallToAppDesc=Choose the applications to install to, platform is 
cn.InstallToAppDesc=选择要安装到的应用程序，平台为

en.InstallFinalMsg=Install to application
cn.InstallFinalMsg=安装到应用程序

; CannotInstallPage [EN]
en.CannotInstallCaption=Administrator privileges required
cn.CannotInstallCaption=需要管理员权限

en.CannotInstallDesc=You do not have the necessary rights to install additional required runtime files.
cn.CannotInstallDesc=您没有安装所需运行时文件的必要权限。

en.CannotInstallMsg=Additional runtime files from Microsoft are required to run this add-in. You may continue the installation, but the add-in won't start unless the required runtime files are installed by an administrator. Note: On Windows Vista and newer, right-click the installer file and choose 'Run as administrator'.
cn.CannotInstallMsg=需要从Microsoft下载运行时文件才能运行此附加组件。您可以继续安装，但是除非您确保安装了运行时文件，否则附加组件将无法正常运行。注意：在Windows Vista及更高版本中，右键单击安装程序文件，然后选择“以管理员身份运行”。

en.CannotInstallCont=Continue anyway, although it won't work without the required runtime files
cn.CannotInstallCont=继续安装，尽管它可能不会正常工作

en.CannotInstallAbort=Abort the installation (come back when the admin has installed the files)
cn.CannotInstallAbort=中止安装（返回，直到管理员安装了文件）

; DownloadInfoPage [EN]
en.RequiredCaption=Additional runtime files required
cn.RequiredCaption=需要额外的运行时文件

en.RequiredDesc=Additional runtime files for the .NET framework from Microsoft are required in order to run this add-in.
cn.RequiredDesc=需要从Microsoft下载.NET框架的额外运行时文件才能运行此附加组件。

en.RequiredMsg=%d file(s) totalling about %s MiB need to be downloaded from the Microsoft servers. Click 'Next' to start downloading.
cn.RequiredMsg=需要从Microsoft服务器下载%d个文件，总计约%s MiB。单击“下一步”开始下载。

; InstallInfoPage [EN]
en.InstallCaption=Runtime files downloaded
cn.InstallCaption=运行时文件已下载

en.InstallDesc=The required runtime files are ready to install.
cn.InstallDesc=所需的运行时文件已准备好安装。

en.InstallMsg=Click 'Next' to beginn the installation.
cn.InstallMsg=单击“下一步”开始安装。

en.StillNotInstalled=The required additional runtime files are still not installed. Setup will continue, but unless you ensure that the runtimes are properly installed, the add-in will not function properly.
cn.StillNotInstalled=所需的额外运行时文件仍未安装。安装程序将继续，但是除非您确保安装了运行时文件，否则附加组件将无法正常运行。

en.DownloadNotValidated=A downloaded file has unexpected content. It may have not been downloaded correctly, or someone might have hampered with it. You may click 'Back' and then 'Next' to download it again.
cn.DownloadNotValidated=下载的文件具有意外的内容。它可能没有正确下载，或者可能已被篡改。您可以单击“后退”，然后单击“下一步”再次下载。

[UninstallDelete]
Type: filesandordirs; Name: "{app}"