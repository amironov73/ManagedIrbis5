unit irbis_01;

{$RANGECHECKS OFF}

interface

uses windows,sysutils,classes;

{������� ��������� � ���������� ������������� �����!!!!!!!}
{��������!!!!!!MAZOV}
const
//paralel ����������� ��������� ������������ MST XRF  1.07.2015
MAX_MFN_PORTION = 32;

VER = 14;//������� � 8.1 ���������� ��� (NOW - 2000) + 0.(����� ������ - 1 2 3 4)
//VER = 7.2;//������� � 8.1 ���������� ��� (NOW - 2000) + 0.(����� ������ - 1 2 3 4)
VERSION = '2012.1';

//14 version
LK_NUM = 10;

NET_REPRESANTATION = 1;{���� � net �������������?}
UTF8_REPRESANTATION = 1;{���� � UTF8 �������������?}
UTF8_FORMAT_FLAG = '!';//���� ������ ������ ������� �� �������������� � UTF8

//��������� ���������� ������ ���������� ��� ���������� ������
//������ ��� ������ >= 8.1
MST_INCREMENT_FILESIZE = 1048576;//��������� ����� �����
XRF_INCREMENT_FILESIZE = 32768;

ZERO = 0;{0 - ���������� ����������}


REC_DELETE = -603;{1 - ������ ��������� �������}
REC_PHYS_DELETE = -605;{2 - ������ ��������� �������}
ERR_RECLOCKED = -602;{������ ������������� �� ����}
AUTOIN_ERROR = -607;//������ autoin.gbl
VERSION_ERROR = -608;//��� ������ ���������� ��������������� ������

ERROR_MEMALLOC = -100;{3 - ������ ��������� ������}
ERR_SHELF_SIZE = -101;{������ ����� ������ ������� ������}
ERR_SHELF_NUMBER = -102;{����� ����� ������ ����� �����}

ERR_POLE_ABSENT = -200;   {��������� ���� ��� isisfldrep irbisfldadd = ������ ����}
ERR_OLDREC_ABSENT = -201;{��� ������ �����}

ERR_DBEWLOCK = -300;{����������� ���������� ��}
ERR_DBDELOCK = -301;{���������� ����� - �� ������������ � IRBIS64!!!!!!!}

READ_WRONG_MFN = -140;{-1 - �������� MFN ��� �������� ��}
READ_WRONG = -141;//������ ������ ������ ��� ������� ����������� ��������!!!!
ERR_FILEMASTER = -400;{������ ��� �������� ����� mst ��� xrf}
ERR_FILEINVERT = -401;{������ ��� �������� trm ������ }
ERR_WRITE = -402;{������ ��� ������}
ERR_ACTUAL = -403;{������ ��� ������������}

ERR_BKP_CREAT = -700;{������ ��� �������� �����}
ERR_BKP_RESTORE = -701;
ERR_LNK_SORT = -702;{������ ��� ����������}
ERR_REC_TERMS = -703;{������ ��� ������ �������� �������}
ERR_LNK_CREAT = -704;{������ ��� ��������� �������}
ERR_LNK_LOAD = -705;{������ ��� �������� �������}
{�������� IRBISFIND}
TERM_NOT_EXISTS = -202;
TERM_LAST_IN_LIST = -203;
TERM_FIRST_IN_LIST = -204;

{������� ���������� �������������}
ERR_GBL_PARAM = -800; {���������� ���������� �� ����� }
ERR_GBL_REP  = -801; {���������� ������ �� ������ }
ERR_GBL_MET  = -802; {����� ������ �� ������}


//MAXBUFFER = 2; {�� ������� ��� ������ ��� ������� ������ ������� ��� ������ = ShelfSize}
//INCREASE_SHELF_SIZE = 2;{��� �������� ������ ����� ������������� ReAllocMem}

MAXRECORD = 32000;{����� ������ (������ �����) ��� IrbisInit ��� �� ������ - 32000!!!!!}
//MAXFIELD = 8000;
//MAX_IFP_REC = 0;{������ ����� ifp}
SIZE_OF_TERMS_BLOCK = 2048;{������ ����� l01 n01}
SIZE_OF_IFP_BLOCK = 4096;{min ������ ����� ifp}
MIN_POSTINGS_IN_BLOCK = 256;{min ���-�� ������ ��� ���� �����}
IFP_BLOCK_SPECIAL = -1001;{������ ifp_low � ��������� ������������ �����}
IFP_LAST_SEGMENT = -1;{������� ���������� ��������}
//1.07.2008
IFP_MAXFILESIZE_BLOCK  = -2;{������� ������������ ����� ��� �������� ��������� ���� � ������ �������}

{SHORTTERM = 30;}

LONGTERM = 255;

//Base_Code_Dos = 0;{��������� ���� ������}
OCCSEP='%';
PRIZUNICODE = '&&';

{���������}
{����� ����� XRF �� ������� � ����!!!!!!!}


{������� ����� ��� ������ � ������� XRF � MST}

BIT_ALL_ZERO = 0;
BIT_LOG_DEL = 1;{��������� ������}
BIT_PHYS_DEL = 2;{��������� ������}
BIT_ABSENT = 4;{�� ����������}
BIT_NOTACT_REC = 8;
{BIT_NEW_REC = 16; {????? ����� �� ????? ������ ����� ������ ����� �������!!!!!}
BIT_LAST_REC = 32;
BIT_LOCK_REC = 64;

AUTOIN_ERROR_FLAG = 128;
{FLAGS}
{--------XRFMFP--------}
{BIT_LOG_DEL  = ��������� ��������� ������}
{BIT_PHYS_DEL = ��������� ��������� ������}
{BIT_ABSENT   = �������������� ������}
{BIT_NOTACT_REC  = ������������������� ������}
{BIT_LOCK_REC = ��������������� ������}
{--------MSTSTATUS--------}
{BIT_LOG_DEL  = ��������� ��������� ������}
{BIT_PHYS_DEL = ��������� ��������� ������}
{BIT_LAST_REC = ��������� ��������� ������}
{BIT_NOTACT_REC  = ������������������� ������}


{
����� ��������� �������

Header_block
------------
Number :integer; ����� ����� ��� 1-�� ����� - ����� ��������� �����
next :integer;
prev :integer;
terms :smallint; ����� �������� � �����
offset_free :smallint; �������� �� ��������� ������� � �����
offset_free-terms*sizeof(THeader_Key)-sizeof(THeader_block) = ��������� b � �����
>>>>>>>>>>>>>>>>>>>>>>>
Header_key
----------
len :smallint; ����� �����
offset_key :smallint; �������� �� ����
block :integer; =0 ������ ; >0 ����� ����� � Nfile ; <0 ����� ����� � Lfile
flag :integer; �� ������� ����� ������� ������ >2 Gb

��������� �����  - Header_block+Header_Key1+...+Header_KeyN+/FREESPACE/+KeyN+...+Key1;
������ �����  - 2048b
������������ ������ ����� L01 - 4000 Gb = sizeof(integer)*������ �����
������������ ����� �������� � ����� - 255
������������ ����� ������� - 255
������� ����� �������� � ����� - 100
�������� ����� ������� � ����� - ������� ����� ����� - 7

������ ������ N01 ��� ����� 1 �����
}
{����������� ������ ���������� � ������������ ��������}
const AllocPortion=1000;

type

TPList = class;
TIntList = class;

//64 �������� ��� ���� ������
PInt64 = ^TInt64;
TInt64 = packed record
   LowWord:DWORD;//����� ����� � ������� � �������� �� �������
   HiWord:DWORD;
   end;
{Term}
//��� ��������� ������ ����� = ������ �����*sizeof(DWORD) ~ 4Tb
    Tcnt_rec = record
    posrx:longint;//����� ����� ������� ������
    end;

    Pcnt_rec = ^Tcnt_rec;
    PHeader_Block=^THeader_Block;
    THeader_Block=packed record
    number :longint;
    prev:longint;
    next:longint;
    terms:word;
    offset_free:word;
    end;

    PHeader_Key=^THeader_Key;
    THeader_Key = packed record
    len:word;
    offset_key:word;
    nxt:TInt64;//������ 24.10.2002
//    block:longword;//������� ����� �������� �� L01 � IFP
//    flag:longword;//������� ����� �������� �� L01 � IFP
    end;

    Tn01 = packed array [0..SIZE_OF_TERMS_BLOCK-1] of char;
    Tl01 = Tn01;
    Pn01 = ^Tn01;
    Pl01 = ^Tl01;

//1.07.2008 �������� �� ��������� ����� � ifp + ������ ��������� ������ � n01 l01
//������ getfilesize!!!!!
    Pifp_MaxFileSize = ^Tifp_MaxFileSize;
    Tifp_MaxFileSize=packed record
    nxt:TInt64;//��������� ����� � ifp
    N01MaxNumber: longint;
    L01MaxNumber: longint;
    reserv: longint;
    end;

    Pifp = ^Tifp;
    Tifp=packed record
//    nxt_high: longword;{next}//<0 ����� �������� ���� ����������  IFP_LAST_SEGMENT = -1
//    nxt_low: longword;{next}
    nxt:TInt64;//������ 24.10.2002
    totp: longint;
    segp: longint;
    segc: longint;
    end;

    TifpItemPosting=packed record
    pmfn:longint;
    ptag:longint;
    pocc:longint;
    pcnt:longint;
    end;
{������ � ����������� ��������� ����� ifp ���� �� 4*N ��� ���������
�� ����� 4,8,16,32 kb ��� ������ ����� ��������� 2048-32000,32000-64000,64000-128000,� �����}
    {TIfp ��� ������������ ����� �������� -
    totp - ����� ���������
    segp - ��������� � ����������� �����
    segc - ������ ������������ ����� � ���������}
    PifpSPecial = ^TIfpSpecial;
    TifpSpecial = packed record
    posting:TifpItemPosting;{1-� ������� � �����}
    nxt:TInt64;//������ 24.10.2002
//    nxt_high: longword;{�������� �� ����}
//    nxt_low: longword;
    end;
    PifpItemPosting = ^TifpItemPosting;
    TifpArr=packed array[1..1]  of TifpItemPosting;
    PifpArr=^TifpArr;{��� ������ ���� ��������� � ����� �����}

    TInvContext = packed record
    FST,FSTDop,STW:TPList;
    FUCTABShkala: {shortstring;}packed array[0..255] of char;
    FACTABShkala:packed array[0..255] of char;
    FDeflex: boolean;
    UnicodeEmulList:TPList;//NEW ��������� unicode �� ����� unicode.tab
    FST_NEW,FSTDop_NEW:TPList; //ALIO ***NEWFST
    FSTKEY1_NEW,FSTKEY2_NEW: TIntList; //ALIO ***NEWFST
    end;

//    Plongint=^longint;
    PSpaceTrm=^TSpaceTrm;

    TSpaceTrm = record
    fdn01,fdl01,fdifp: smallint;
    n01rec: Tn01;
    l01rec: Tl01;

    cntrec: Tcnt_rec;
    ifp: Tifp;
    curifp_posting:TifpItemPosting;{������� �������}
    curpost:integer;{������� ������ ��������}
    NumberPosting:integer;{����� ���������}
    Level:TIntList;{������ ������ ��� �������� � ������ �� ������� ��������� ������ � L ����}
    //14 ������ - ����������� ������ ���������

    trm_postings:PifpArr; //��������  �� ������ ����� ifp
    trm_postings_size:integer; //����� ���������� ������ � ��������� TifpItemPosting

    end;

{mst}
    Pcntmst=^Tcntmst;
    Tcntmst=packed record
    ctlmfn: longint;
    nxtmfn: longint;
    nxt:TInt64;//������ 24.10.2002
//    nxt_low: longword;
//    nxt_high: longword;{��������!!!!!!MAZOV}
    netdb: longint;{��������!!!!!!MAZOV}//���� net ������������� ��
    reccnt: longint;//����� ���������� 1.07.2015
    mfcxx1: longint;//������
    mfcxx2: longint;//������������ - ������ ��������� (����� �������) MST XRF 29.06.2015
    mfcxx3: longint;
    end;

    Plider=^Tlider;
    Tlider=packed record
    mfn: longint;
    mfrl:longint;
    nxt:TInt64;//������ 24.10.2002
//    mfb_low: longword;
//    mfb_high: longword;{��������!!!!!!MAZOV}
    base: longint;
    nvf: longint;
    version:longint;//����� ������ 2003_04
    status: longint;{��������!!!!!!MAZOV}    
    end;

    Penter=^Tenter;
    Tenter=packed record
    tag: longint;
    pos: longint;
    len: longint;
    end;

    PNew_xrf=^TNew_xrf;
    TNew_xrf=packed record
    nxt:TInt64;//������ 24.10.2002
//    xrf_low:longword;
//    xrf_high:longword;{��������!!!!!!MAZOV}
    xrf_flags:longint;{��������!!!!!MAZOV}
    end;


    TShelf=packed record
    recordbuf, {����� ������ � ����������� �������� ������}
    bodybuf: {����� ���� �������� ������}
          Pchar;
    fieldbuf:PChar; {����� ����}
    Xrf:TNew_Xrf;{������ �� xrf}
//    IsActualized:longint;{������� ������������}
//    IsNew:longint;{����� ������}
//    IsLocked:longint;{���������� ������}
//    Version:longint;//����� ������ 2003_04
    ShelfSize:longint;{init ������ �����}
    end;

    //������������ MST XRF  1.07.2015
    TMSTXRF_Handles=packed record
      mst:THandle;
      xrf:THandle;
      cnt:Tcntmst;
      end;

    PShelfs=^TShelfs;
    TShelfs=packed array[0..0] of TShelf;

    PSpaceMst = ^TSpaceMst;
    TSpaceMst = packed record
    fdmst,fdxrf: THandle;
    cntmst: Tcntmst;
    Shelfs:PShelfs;
    end;
    PIrbisSpace=^TIrbisSpace;
    TIrbisSpace = packed record
    PSM:PSpaceMst;
    //������������ MST XRF  1.07.2015
    MSTXRF:array [1..MAX_MFN_PORTION] of TMSTXRF_Handles;
    //14 version
    //6.09.2013 ������ ��������� ������
    PST_INV:array [0..LK_NUM]of PSpaceTrm;//PST_INV[0] = PST ��������� �� ������� �� 14 ������ ifp n01 l01 �����
    INV_OPEN:boolean;//���� ���������������� ������� � InvList.count=LK_NUM
    INV_INDEX:integer;//������� ������ ���������� ����� - ����������� � ������� find
    InvList:TPList;//��������� ������� ��� �������������� ����� �������

    InvContext:TInvContext;
    MstPath:PChar;{inittmst}
    IfpPath:PChar;{initterm}
    FstPath:PChar;{initInvContext}
    StwPath:PChar;{initInvContext}
    PFTPath:PChar;{initpft}
    {��������� ���� ������������ � ������� ������}
    Lock:integer;{������� ������� ���������� ��}
    recbuffer:Pchar;{������������ ������ ��� �������� �������� ������}
    SizeBuffer:longint;
    reclength:longint;{����� ������ ����� isisread}
    xrf:TNew_Xrf;{������ �� xrf}
//    IsActualized:longint;
//    IsLocked:longint;{���������� ������}
//    Version:longint;//����� ������ 2003_04

    NumShelfs:longint;{initmst ����� ����� � PSM}
//    ShelfSize:longint;{init ������ �����}
    workerbuf:PChar;  {������� �������}
    pftbuf:Pchar;   {������� �������}
    PFT_SIZE:integer;
    WORKER_SIZE:integer;
    end;

    TSortType=(srtDown,srtUp);
    TIrbisKey = packed array[0..LONGTERM] of char;

    TArrayInteger=packed array[0..0] of integer;
    PArrayInteger=^TArrayInteger;

    TIntList=class
    private
    MfnList: PArrayInteger;
    MfnListCapacity: integer;
    MfnListCount: integer;
    Fportion:integer;
    function  Get(Index: Integer): integer;
    procedure Put(Index: Integer; Item: integer);
    procedure SetCapacity(NewCapacity: Integer);
//    procedure SetCount(NewCount: Integer);
    public
    constructor Create;
    destructor Destroy; override;
    function IndexOf(AItem: integer): integer;
    function Add(AItem: integer): integer;
    procedure Sort(ASortType:TSortType);
    function Find(const S: integer; var Index: Integer; SortType:TSortType): Boolean;
    procedure Append(Source:TIntList);
    procedure Assign(Source:TIntList);
    function Insert(Index: Integer; Item: integer):integer;
    function Delete(Index: Integer):integer;
    procedure Clear;
    procedure LoadFromFile(line:PChar);
    procedure SaveToFile(line:PChar);
    property Capacity: Integer read MfnListCapacity;
    property Count: Integer read MfnListCount;
    property Portion:integer read FPortion;
    property Items[Index: Integer]: integer read Get write Put; default;
    end;


    TIfpItemPostings = packed record
      posting:TifpItemPosting;
      FObject:integer;
      end;

    PArrayPostings=^TArrayPostings;
    TArrayPostings=packed array[0..0] of TIfpItemPostings;

    TIfpPostingList = class
    public
    FPostings:PArrayPostings;
    FCapacity:integer;
    FCount:integer;
    FPortion:integer;
    constructor Create;
    destructor Destroy;override;
    function Add(Item:TIfpItemPosting):integer;
    function AddObject(Item:TIfpItemPosting;AObject:integer):integer;
    function Get(Index:integer):TIfpItemPosting;
    procedure Put(Index:integer;Item:TIfpItemPosting);
    function GetObject(Index:integer):integer;
    procedure PutObject(Index:integer;Item:integer);
    procedure Clear;
    property Items[Index: Integer]:TIfpItemPosting read Get write Put; default;
    property Objects[Index:integer]:integer read GetObject write PutObject;
    property Count:integer read FCount;
    property Portion:integer read FPortion write FPortion;
    end;

{}
{������ TStringList!!!!!!!}
    PListItem = ^TListItem;
    TListItem = record
       FObject:integer;
       FItem:PChar;
       end;

    TPArray=array[0..0] of TListItem;
    PPArray=^TPArray;

    TPList=class
    public
    FList: PPArray;
    FCapacity: integer;
    FCount: integer;
    Fportion:integer;
    function  Get(Index: Integer): PChar;
    procedure Put(Index: Integer; Item: PChar);
    function  GetObject(Index: Integer): integer;
    procedure PutObject(Index: Integer; Item: integer);
    procedure SetCapacity(NewCapacity: Integer);
    constructor Create;
{    procedure Free;}
    destructor Destroy;override;
    function IndexOf(AItem: PChar): integer;
    function Add(AItem: PChar): integer;
    function AddString(AItem: string): integer;
    procedure AddObject(AItem: PChar;AObject:integer);
    procedure Sort;
    procedure Utf8Sort;
    function Find(const S: PChar; var Index: Integer): Boolean;
    function FindUTF8(const S: PChar; var Index: Integer): Boolean;    
    function My_Compare(T1,T2:PChar;P1,P2:TIfpItemPosting):integer;
    procedure Sort_with_Postings(Postings:TifpPostingList);
    procedure Append(Source:TPList);
    procedure Assign(Source:TPList);
    function Insert(Index: Integer; Item: PChar):integer;
    function Delete(Index: Integer):integer;
    procedure LoadFromFile(line:PChar);
    procedure SaveToFile(line:PChar);
    function GetText:PChar;
    procedure SetText(const Value:PChar);
    procedure Clear;
    function GetTextSize:integer;
    property Capacity: Integer read FCapacity;
    property Count: Integer read FCount;
    property Portion:integer read FPortion write FPortion;
    property Items[Index: Integer]: PChar read Get write Put; default;
    property Objects[Index:Integer]:integer read GetObject write PutObject;
    end;


procedure MoveHeaderBlock(const Source:Pn01;Dest:Pn01);
procedure MoveKey(const Source:Pn01;IndexSource:integer;Dest:Pn01;IndexDest:integer);

//2.12.2013 kostia
function StrToFloat(const S: string): Extended;
implementation

//uses backup;

function StrToFloat(const S: string): Extended;
begin
  if not TextToFloat(PChar(S), Result, fvExtended) then
  Result:=0;
//    ConvertErrorFmt(@SInvalidFloat, [S]);
end;

constructor TIfpPostingList.Create;
begin
FPostings:=AllocMem(AllocPortion*sizeof(TIfpItemPostings));
FCapacity:=AllocPortion;
FCount:=0;
inherited Create;
end;

destructor TIfpPostingList.Destroy;
begin
try
   FreeMem(FPostings);
   except
   end;
inherited Destroy;
end;

procedure TIfpPostingList.Clear;
begin
FCount:=0;
end;

function TIfpPostingList.Add(Item:TIfpItemPosting):integer;
begin
Result:=0;
try
   if (FCount = FCapacity) then
        begin
        ReAllocMem(FPostings,(FCapacity+AllocPortion)*sizeof(TIfpItemPostings));
        FCapacity:=FCapacity+AllocPortion;
        end;
   FPostings[FCount].posting:=Item;
   FCount:=FCount+1;
   except
   Result:=-1;
   end;
end;

function TIfpPostingList.AddObject(Item:TIfpItemPosting;AObject:integer):integer;
begin
Result:=0;
if Add(Item) = 0 then FPostings[FCount-1].FObject:=AObject
else Result:=-1;
end;

function TIfpPostingList.Get(Index:integer):TIfpItemPosting;
begin
if (Index < 0)or(Index > FCount)
  then fillchar(Result,sizeof(TIfpItemPosting),0)
  else Result:=FPostings[Index].posting;
end;

procedure TIfpPostingList.Put(Index:integer;Item:TIfpItemPosting);
begin
if (Index < 0)or(Index > FCount)
  then
  else FPostings[Index].posting:=Item;
end;

function TIfpPostingList.GetObject(Index:integer):integer;
begin
if (Index < 0)or(Index > FCount)
  then fillchar(Result,sizeof(TIfpItemPostings),0)
  else Result:=FPostings[Index].FObject;
end;

procedure TIfpPostingList.PutObject(Index:integer;Item:integer);
begin
if (Index < 0)or(Index > FCount)
  then
  else FPostings[Index].FObject:=Item;
end;

procedure MoveHeaderBlock(const Source:Pn01;Dest:Pn01);
begin
Move(Source[0],Dest[0],sizeof(THeader_Block));
end;

procedure MoveKey(const Source:Pn01;IndexSource:integer;Dest:Pn01;IndexDest:integer);
begin
{������� ��������� �� ����}
Move(Source[sizeof(THeader_Block)+(IndexSource-1)*sizeof(THeader_key)],
     Dest[sizeof(THeader_Block)+(IndexDest-1)*sizeof(THeader_key)],
     sizeof(THeader_Key));
{�������� �� ����� ����� ������ ���������}
PHeader_Block(Dest).offset_free:=
PHeader_Block(Dest).offset_free - PHeader_Key(PChar(Source)+sizeof(THeader_Block)+(IndexSource-1)*sizeof(THeader_Key)).len;
{�������� �� ���� ������ ��������� � �������� �������� �� ����� �����}
PHeader_Key(PChar(Dest)+sizeof(THeader_Block)+(IndexDest-1)*sizeof(THeader_Key)).offset_key:=PHeader_Block(Dest).offset_free;
{������� �����}
Move(Source[PHeader_Key(PChar(Source)+sizeof(THeader_Block)+(IndexSource-1)*sizeof(THeader_Key)).offset_key],
     Dest[PHeader_Key(PChar(Dest)+sizeof(THeader_Block)+(IndexDest-1)*sizeof(THeader_Key)).offset_key],
     PHeader_Key(PChar(Source)+sizeof(THeader_Block)+(IndexSource-1)*sizeof(THeader_Key)).len);
PHeader_Block(Dest).terms:=PHeader_Block(Dest).terms+1;
end;

{IntList,PList}
function TIntList.IndexOf(AItem: integer): integer;
var ri: integer;
begin
Result:=-1;
for ri:=0 to MfnListCount-1 do
    begin
    if AItem=MfnList[ri] then
       begin
       Result:=ri;
       exit;
       end;
    end;
end;


function TIntList.Find(const S: integer; var Index: Integer; SortType:TSortType): Boolean;
var
  L, H, I, C: Integer;
begin
  Result := False;
  Index:=-1;
  L := 0;
  H := MfnListCount - 1;
  while L <= H do
  begin
    I := (L + H) shr 1;
//    C := StrComp(FList^[I].FItem, S);
    if(SortType = srtUp)
      then C := MFNList[I] - S
      else C := S - MFNList[I];
    if C < 0 then L := I + 1 else
    begin
      H := I - 1;
      if C = 0 then
      begin
        Result := True;
        L := I;//��������� �� �������������!!!
//        if Duplicates <> dupAccept then L := I;
      end;
    end;
  end;
  Index := L;
end;


function TIntList.Add(AItem: integer): integer;
begin
Result:=0;
try
   if Count=MfnListCapacity then
        begin
        ReAllocMem(MfnList,(MfnListCapacity+FPortion)*sizeof(integer));
        MfnListCapacity:=MfnListCapacity+FPortion;
        end;
   MfnList[MfnListCount]:=AItem;
   MfnListCount:=MfnListCount+1;
   except
   Result:=-1;
   end;
end;

procedure TIntList.Sort(ASortType:TSortType);

    procedure QuickSort(SortList: PArrayInteger; L, R: Integer;SortType:TSortType);
    var
    I, J: Integer;
    P, T: Integer;
    begin
     repeat
       I := L;
       J := R;
       P := SortList[(L + R) shr 1];
       repeat
         case TSortType(SortType) of
            srtDown:  begin
                     while (SortList[I]>P) do Inc(I);
                     while (SortList[J]< P) do Dec(J);
                    end;
            srtUp:begin
                     while (SortList[I]< P) do Inc(I);
                     while (SortList[J]> P) do Dec(J);
                    end;
            end;
         if I <= J then
         begin
           T := SortList[I];
           SortList[I] := SortList[J];
           SortList[J] := T;
           Inc(I);
           Dec(J);
         end;
       until I > J;
       if L < J then QuickSort(SortList, L, J, SortType);
       L := I;
     until I >= R;
   end;

begin
  if (MfnList <> nil) and (MfnListCount > 0) then
    QuickSort(MfnList, 0, MfnListCount - 1,ASortType );
end;


constructor TIntList.Create;
begin
FPortion:=AllocPortion;
GetMem(MfnList,Fportion*SizeOf(integer));
MfnListCapacity:=FPortion;
MfnListCount:=0;
end;

destructor TIntList.Destroy;
begin
try
  FreeMem(MfnList);
  except
  end;
  inherited Destroy;
end;

function TIntList.Get(Index: Integer): integer;
begin
  if (Index < 0) or (Index >= MfnListCount) then Result:=-1
     else Result := MfnList[Index];
end;

procedure TIntList.Put(Index: Integer; Item: integer);
begin
  if (Index < 0) or (Index >= Count) then exit;
  MfnList[Index] := Item;
end;

procedure TIntList.SetCapacity(NewCapacity: Integer);
begin
  if (NewCapacity < Count)  then exit;
  if NewCapacity <> MfnListCapacity then
  begin
    ReallocMem(MfnList, NewCapacity * SizeOf(integer));
    MfnListCapacity := NewCapacity;
  end;
end;
{
procedure TIntList.SetCount(NewCount: Integer);
begin
  if (NewCount < 0)  then exit;
  if NewCount > MfnListCapacity then SetCapacity(NewCount);
  if NewCount > Count then
    FillChar(MfnList[MfnListCount], (NewCount - MfnListCount) * SizeOf(integer), 0);
  MfnListCount := NewCount;
end;
}
procedure TIntList.Clear;
begin
{
  SetCount(0);
  SetCapacity(0);
}
{
FreeMem(MfnList);

FPortion:=AllocPortion;
GetMem(MfnList,Fportion*SizeOf(integer));
MfnListCapacity:=FPortion;
MfnListCount:=0;
}
FillChar(MfnList[0],sizeof(integer)*MfnListCapacity,0);
MfnListCount:=0;
end;

procedure TIntList.Append(Source:TIntList);
var ri:integer;
begin
if Source=nil then exit;
for ri:=0 to Source.count-1 do
  Add(Source[ri]);
end;

procedure TIntList.Assign(Source:TIntList);
var ri:integer;
begin
if Source=nil then exit;
Clear;
for ri:=0 to Source.count-1 do
  Add(Source[ri]);
end;

function TIntList.Insert(Index: Integer; Item: integer):integer;
begin
  Result:=-1;
  if (Index < 0) or (Index > MfnListCount) then exit;
  if MfnListCount = MfnListCapacity then SetCapacity(MfnListCapacity+FPortion);
  if Index < MfnListCount then
    System.Move(MfnList[Index], MfnList[Index + 1],
      (MfnListCount - Index) * SizeOf(integer));
  MfnList[Index] := Item;
  Inc(MfnListCount);
  Result:=0;
end;

function TIntList.Delete(Index: Integer):integer;
begin
  Result:=-1;
  if (Index < 0) or (Index >= MfnListCount) then exit;
  Dec(MfnListCount);
  if Index < MfnListCount then
    System.Move(MfnList[Index + 1], MfnList[Index],
      (MfnListCount - Index) * SizeOf(integer));
  Result:=0;
end;


function OpenTerm_x(filename:PChar;var FileMapping:THandle;var HighSize:integer):PChar;
var
hFile:THandle;
begin
try
Result:=nil;
hFile:=_lopen(FileName,OF_READ+OF_SHARE_DENY_NONE); //ALIO 04.08.2011 �������� +OF_SHARE_DENY_NONE
if integer(hFile)<=0 then exit;
HighSize:=GetFileSize(hFile,nil);{PAGE_READONLY}
result:=AllocMem(HighSize+1);
_llseek(hFile,0,0);
_lread(hFile,result,HighSize);
_lclose(hFile);
except
Result:=nil;
end;
end;

procedure CloseTerm_x(FileMapping:THandle;OpenFile:PChar);
begin
try
FreeMem(OpenFile);
except
end;
end;


procedure TIntList.LoadFromFile(line:PChar);
var
Stream:PChar;
rs:string;
P, Start: PChar;
FileMap:THandle;
FileSize:integer;

begin
{  Stream := TFileStream.Create(string(line), fmOpenRead);}
  Stream:=OpenTerm_x(line,FileMap,FileSize);
  try
  Clear;
  P:=Stream;
  if P <> nil then
      while (P^ <> #0) or (P-Stream<FileSize)do
      begin
        Start := P;
        while not (P^ in [#0, #10, #13]) do
           begin
           if P-Stream>=FileSize then break;
           Inc(P);
           end;
        SetString(RS, Start, P - Start);
        Add(StrToInt(RS));
        if P^ = #13 then Inc(P);
        if P^ = #10 then Inc(P);
      end;
    finally
    CloseTerm_x(FileMap,Stream);
    end;
end;

procedure TIntList.SaveToFile(line:PChar);
var
  I, L, Size, Count: Integer;
  P: PChar;
  RS,S: string;
  OutFile:smallint;
begin
  OutFile:=_lcreat(line,0);
  if OutFile<=0 then exit;
  Size := 0;
  try
  Count := MfnListCount;
  for I := 0 to Count - 1 do Inc(Size, Length(IntToStr(Get(I))) + 2);
  SetString(S, nil, Size);
  P := Pointer(S);
  for I := 0 to Count - 1 do
  begin
    RS := IntToStr(Get(I));
    L := Length(RS);
    if L <> 0 then
    begin
      System.Move(Pointer(RS)^, P^, L);
      Inc(P, L);
    end;
    P^ := #13;
    Inc(P);
    P^ := #10;
    Inc(P);
  end;
  finally
  _lwrite(OutFile,PChar(S),Size);
  _lclose(OutFile);
  end;
end;

{PList}
function TPList.IndexOf(AItem: PChar): integer;
var ri: integer;
begin
Result:=-1;
if AItem = nil then exit;
for ri:=0 to Count-1 do
    begin
    if FList[ri].FItem = nil then continue;
    if StrComp(AItem,FList[ri].FItem)=0 then
       begin
       Result:=ri;
       exit;
       end;
    end;
end;

function TPList.Add(AItem: PChar): integer;
begin
Result:=0;
try
   if FCount=FCapacity then
        begin
        ReAllocMem(FList,(FCapacity+FPortion)*sizeof(TListItem));
        FCapacity:=FCapacity+FPortion;
        end;
   if (AItem=nil)or(strlen(AItem) = 0)
      then FList[FCount].FItem:=AllocMem(1){!!!!!#0}
      else begin
           FList[FCount].FItem:=AllocMem(strlen(AItem)+1);
           if strlen(AItem) > 0 then Move(AItem[0],PChar(FList[FCount].FItem)[0],strlen(AItem));
           PChar(FList[FCount].FItem)[strlen(AItem)]:=#0;
           end;
   FCount:=FCount+1;
   except
   Result:=-1;
   end;
end;


function TPList.AddString(AItem: string): integer;
begin
Result:=0;
try
   if FCount=FCapacity then
        begin
        ReAllocMem(FList,(FCapacity+FPortion)*sizeof(TListItem));
        FCapacity:=FCapacity + FPortion;
        end;

   if AItem = ''
      then begin
           FList[FCount].FItem:=AllocMem(1);{!!!!!#0}
           end
      else begin
           FList[FCount].FItem:=AllocMem(length(AItem)+1);
           if length(AItem) > 0 then Move(PChar(AItem)[0],PChar(FList[FCount].FItem)[0],length(AItem));
           PChar(FList[FCount].FItem)[length(AItem)]:=#0;

           end;
   FCount:=FCount+1;
   except
   Result:=-1;
   end;
end;

function Utf8Decode(const S: String): WideString;

    function Utf8ToUnicode(Dest: PWideChar; MaxDestChars: Cardinal;
      Source: PAnsiChar; SourceBytes: Cardinal): Integer;
    var
      i, count: Cardinal;
      c: Byte;
      wc: Cardinal;
    begin
      if Source = nil then
      begin
        Result := 0;
        Exit;
      end;
      Result := -1;
      count := 0;
      i := 0;
      if Dest <> nil then
      begin
        while (i < SourceBytes) and (count < MaxDestChars) do
        begin
          wc := Cardinal(Source[i]);
          Inc(i);
          if (wc and $80) <> 0 then
          begin
            if i >= SourceBytes then Exit;          // incomplete multibyte char
            wc := wc and $3F;
            if (wc and $20) <> 0 then
            begin
              c := Byte(Source[i]);
              Inc(i);
              if (c and $C0) <> $80 then Exit;      // malformed trail byte or out of range char
              if i >= SourceBytes then Exit;        // incomplete multibyte char
              wc := (wc shl 6) or (c and $3F);
            end;
            c := Byte(Source[i]);
            Inc(i);
            if (c and $C0) <> $80 then Exit;       // malformed trail byte

            Dest[count] := WideChar((wc shl 6) or (c and $3F));
          end
          else
            Dest[count] := WideChar(wc);
          Inc(count);
        end;
        if count >= MaxDestChars then count := MaxDestChars-1;
        Dest[count] := #0;
      end
      else
      begin
        while (i < SourceBytes) do
        begin
          c := Byte(Source[i]);
          Inc(i);
          if (c and $80) <> 0 then
          begin
            if i >= SourceBytes then Exit;          // incomplete multibyte char
            c := c and $3F;
            if (c and $20) <> 0 then
            begin
              c := Byte(Source[i]);
              Inc(i);
              if (c and $C0) <> $80 then Exit;      // malformed trail byte or out of range char
              if i >= SourceBytes then Exit;        // incomplete multibyte char
            end;
            c := Byte(Source[i]);
            Inc(i);
            if (c and $C0) <> $80 then Exit;       // malformed trail byte
          end;
          Inc(count);
        end;
      end;
      Result := count + 1;
    end;

var
  L: Integer;
  Temp: WideString;
begin
  Result := '';
  if S = '' then Exit;
  SetLength(Temp, Length(S));

  L := Utf8ToUnicode(PWideChar(Temp), Length(Temp)+1, PAnsiChar(S), Length(S));
  if L > 0 then
    SetLength(Temp, L-1)
  else
    Temp := '';
  Result := Temp;
end;


function UTF8ToWideString(const S: AnsiString): WideString;
begin
  Result := UTF8Decode(S);
end;

function keycmpUtf8(key1,key2: Pchar; len1,len2: integer):   integer;
var ri,size: integer;
rs1,rs2:String;
ws1,ws2:WideString;
lenw1,lenw2:integer;
begin
//unicode
SetString(rs1,key1,len1);
SetString(rs2,key2,len2);
ws1:=Utf8ToWIdeString(rs1);
ws2:=Utf8ToWIdeString(rs2);
lenw1:=length(ws1);
lenw2:=length(ws2);
result:=0;
if lenw1>lenw2 then size:=lenw2 else size:=lenw1;
        for ri:=0 to size-1 do
            begin
            if PWideChar(ws1)[ri]<PWideChar(ws2)[ri]
               then begin
                    result:=-1;
                    break;
                    end
               else begin
                    if PWideChar(ws1)[ri]>PWideChar(ws2)[ri] then
                       begin
                       result:=1;
                       break;
                       end
                    end;
            end;
        if (result = 0) and (lenw1<>lenw2)
          then if lenw1<lenw2
                    then result:=-1 else result:=1;
end;


procedure TPList.Utf8Sort;

    procedure QuickSort(SortList: PPArray; L, R: Integer);
    var
    I, J: Integer;
    P, T: PChar;
    E: integer;
    begin
     repeat
       I := L;
       J := R;
       P := SortList[(L + R) shr 1].FItem;
       repeat
//            while StrComp(SortList[I].FItem,P)<0 do Inc(I);
//            while StrComp(SortList[J].FItem,P)>0 do Dec(J);
              while keycmpUtf8(SortList[I].FItem,P,strlen(SortList[I].FItem),strlen(P))<0 do Inc(I);
              while keycmpUtf8(SortList[J].FItem,P,strlen(SortList[J].FItem),strlen(P))>0 do Dec(J);
         if I <= J then
         begin
           T := SortList[I].FItem;
           SortList[I].FItem := SortList[J].FItem;
           SortList[J].FItem := T;
           E := SortList[I].FObject;
           SortList[I].FObject := SortList[J].FObject;
           SortList[J].FObject := E;
{           ExchangeItems(SortList,I,J);}
           Inc(I);
           Dec(J);
         end;
       until I > J;
       if L < J then QuickSort(SortList, L, J);
       L := I;
     until I >= R;
   end;

begin
  if (FList <> nil) and (FCount > 0) then
    QuickSort(FList, 0, FCount - 1);
end;

procedure TPList.Sort;

    procedure QuickSort(SortList: PPArray; L, R: Integer);
    var
    I, J: Integer;
    P, T: PChar;
    E: integer;
    begin
     repeat
       I := L;
       J := R;
       P := SortList[(L + R) shr 1].FItem;
       repeat
            while StrComp(SortList[I].FItem,P)<0 do Inc(I);
            while StrComp(SortList[J].FItem,P)>0 do Dec(J);
         if I <= J then
         begin
           T := SortList[I].FItem;
           SortList[I].FItem := SortList[J].FItem;
           SortList[J].FItem := T;
           E := SortList[I].FObject;
           SortList[I].FObject := SortList[J].FObject;
           SortList[J].FObject := E;
{           ExchangeItems(SortList,I,J);}
           Inc(I);
           Dec(J);
         end;
       until I > J;
       if L < J then QuickSort(SortList, L, J);
       L := I;
     until I >= R;
   end;

begin
  if (FList <> nil) and (FCount > 0) then
    QuickSort(FList, 0, FCount - 1);
end;


function ComparePostings(p1,p2:TifpItemPosting):integer;
  function Compare(I1,I2:DWORD):integer;
  var A1,A2:cardinal;
  begin
     A1:=I1;
     A2:=I2;
     result:=-1;
     if A1 > A2
        then result:=1
        else if A1 = A2
               then result:=0;
  end;
begin

result:=p1.pmfn - p2.pmfn;
if result = 0
   then result:=p1.ptag - p2.ptag
   else exit;
if result = 0
   then result:=p1.pocc - p2.pocc
   else exit;
if result = 0
   then result:=p1.pcnt - p2.pcnt
   else exit;
{
result:=Compare(p1.pmfn,p2.pmfn);
if result = 0
   then result:=Compare(p1.ptag,p2.ptag);
if result = 0
   then result:=Compare(p1.pocc,p2.pocc);
if result = 0
   then result:=Compare(p1.pcnt,p2.pcnt);
   }
end;

function TPList.My_Compare(T1,T2:PChar;P1,P2:TIfpItemPosting):integer;
begin
result:=StrComp(T1,T2);
if result = 0 then result:=ComparePostings(P1,P2);
end;

procedure TPList.Sort_with_Postings(Postings:TifpPostingList);

    procedure QuickSort(SortList: PPArray;Postings:TIfpPostingList ; L, R: Integer);
    var
    I, J: Integer;
    O :TIfpItemPosting;
    P, T: PChar;
    E: integer;
    begin
     repeat
       I := L;
       J := R;
       P := SortList[(L + R) shr 1].FItem;
       O := Postings.Items[SortList[(L + R) shr 1].FObject];
       repeat
            while My_Compare(SortList[I].FItem,P,Postings.Items[SortList[I].FObject],O)<0 do Inc(I);
            while My_Compare(SortList[J].FItem,P,Postings.Items[SortList[J].FObject],O)>0 do Dec(J);
         if I <= J then
         begin
           T := SortList[I].FItem;
           SortList[I].FItem := SortList[J].FItem;
           SortList[J].FItem := T;
           E := SortList[I].FObject;
           SortList[I].FObject := SortList[J].FObject;
           SortList[J].FObject := E;
{           ExchangeItems(SortList,I,J);}
           Inc(I);
           Dec(J);
         end;
       until I > J;
       if L < J then QuickSort(SortList, Postings, L, J);
       L := I;
     until I >= R;
   end;

begin
  if (FList <> nil) and (FCount > 0) then
    QuickSort(FList, Postings, 0, FCount - 1);
end;

constructor TPList.Create;
begin
FPortion:=AllocPortion;
FList:=AllocMem(Fportion*SizeOf(TListItem));
//19.11.2013
FCapacity:=Fportion;
FCount:=0;
end;

destructor TPList.Destroy;
var ri:integer;
begin
  for ri:=0 to FCount-1 do
     begin
     try
       if FList[ri].FItem <> nil then FreeMem(FList[ri].FItem);
       except
       end;
     end;
  try
    FreeMem(FList);
    except
    end;
  inherited Destroy;
end;

function TPList.Get(Index: Integer): PChar;
begin
  if (Index < 0) or (Index >= FCount) then Result:=nil
  else Result := FList[Index].FItem;
end;

procedure TPList.Put(Index: Integer; Item: PChar);
begin
  if (Index < 0) or (Index >= FCount) then exit;
   if (Item=nil)
      then  begin
            try
              if (FList[Index].FItem <> nil) then FreeMem(FList[Index].FItem);
              finally
              FList[Index].FItem:=nil;
              FList[Index].FObject:=0;
              end;
            end
      else begin
            try
              if (FList[Index].FItem <> nil) then FreeMem(FList[Index].FItem);
              finally
              FList[Index].FItem:=AllocMem(strlen(Item)+1);
              if strlen(Item) > 0 then Move(Item[0],PChar(FList[Index].FItem)[0],strlen(Item));
              PChar(FList[Index].FItem)[strlen(Item)]:=#0;
              end;
           end;
end;


procedure TPList.SetCapacity(NewCapacity: Integer);
begin
  if (NewCapacity < FCount)  then exit;
  if NewCapacity <> FCapacity then
  begin
    ReallocMem(FList, NewCapacity * SizeOf(TListItem));
    FCapacity := NewCapacity;
  end;
end;

procedure TPList.Clear;
var ri:integer;
begin
for ri:=0 to FCount-1 do
    begin
    try
      if (FList[ri].FItem <> nil) then FreeMem(FList[ri].FItem);
      finally
      FList[ri].FItem:=nil;
      FList[ri].FObject:=0;
      end;
    end;
FCount:=0;
end;

procedure TPList.Append(Source:TPList);
var ri:integer;
begin
if Source=nil then exit;
for ri:=0 to Source.count-1 do
  AddObject(Source[ri],Source.Objects[ri]);
end;

procedure TPList.Assign(Source:TPList);
var ri:integer;
begin
if Source=nil then exit;
Clear;
for ri:=0 to Source.count-1 do
  AddObject(Source[ri],Source.Objects[ri]);
end;


function TPList.Insert(Index: Integer; Item: PChar):integer;
begin
  Result:=-1;
  if (Index < 0) or (Index > FCount) then exit;
  if FCount = FCapacity then SetCapacity(FCapacity+FPortion);
  if Index < FCount then
    System.Move(FList[Index], FList[Index + 1],
      (FCount - Index) * SizeOf(TListItem));
  FList[Index].FItem:=AllocMem(strlen(Item)+1);
  FList[Index].FObject:=0;
  if strlen(Item) > 0 then Move(Item[0],PChar(FList[Index].FItem)[0],strlen(Item));
  PChar(FList[Index].FItem)[strlen(Item)]:=#0;
  Inc(FCount);
  Result:=0;
end;

function TPList.Delete(Index: Integer):integer;
begin
  Result:=-1;
  if (Index < 0) or (Index >= FCount) then exit;
  Dec(FCount);
  try
     if (FList[Index].FItem <> nil) then FreeMem(FList[Index].FItem);
     finally
     FList[Index].FItem:=nil;
     FList[Index].FObject:=0;
     end;
  if Index < FCount then
    System.Move(FList[Index + 1], FList[Index],
      (FCount - Index) * SizeOf(TListItem));
  Result:=0;
end;


procedure TPList.LoadFromFile(line:PChar);
var
Stream:PChar;
rs:string;
P, Start: PChar;
FileMap:THandle;
FileSize:integer;

begin
  Stream:=OpenTerm_x(line,FileMap,FileSize);
  try
  Clear;
  P := Stream;
  if P <> nil then
      while (P^ <> #0) and (P-Stream<FileSize) do
      begin
        Start := P;
        while not (P^ in [#0, #10, #13]) do
           begin
           if P-Stream>=FileSize then break;
           Inc(P);
           end;
        SetString(RS, Start, P - Start);
        Add(PChar(RS));
        if P^ = #13 then Inc(P);
        if P^ = #10 then Inc(P);
      end;
    finally
    CloseTerm_x(FileMap,Stream);
    end;
end;

procedure TPList.SaveToFile(line:PChar);
var
  I, L, Size, Count: Integer;
  P: PChar;
  RS,S: string;
  OutFile:smallint;
begin
  OutFile:=_lcreat(line,0);
  if OutFile<=0 then exit;
  Size := 0;
  try
  Count := FCount;
  for I := 0 to Count - 1 do Inc(Size, strlen(Get(I)) + 2);
  SetString(S, nil, Size);
  P := Pointer(S);
  for I := 0 to Count - 1 do
  begin
    RS := string(Get(I));
    L := Length(RS);
    if L <> 0 then
    begin
      System.Move(Pointer(RS)^, P^, L);
      Inc(P, L);
    end;
    P^ := #13;
    Inc(P);
    P^ := #10;
    Inc(P);
  end;
  finally
  _lwrite(OutFile,PChar(S),Size);
  _lclose(OutFile);
  end;
end;

function  TPList.GetObject(Index: Integer): integer;
begin
  if (Index < 0) or (Index >= FCount) then Result:=0
  else Result := FList[Index].FObject;
end;

procedure TPList.PutObject(Index: Integer; Item: integer);
begin
  if (Index < 0) or (Index >= FCount) then exit;
  if (Item<>0) then FList[Index].FObject:=Item;
end;

procedure TPlist.AddObject(AItem: PChar;AObject:integer);
begin
{!!!!!!!!!����� Add FCount ������������� �� 1!!!!!!!!!!}
if Add(AItem)=0 then FList[FCount-1].FObject:=AObject;
end;


function TPList.GetText:PChar;
var
  I, L, Size, Count: Integer;
  P: PChar;
  S: PChar;
begin
  Count := FCount;
  Size := 0;
  for I := 0 to Count - 1 do Inc(Size, strlen(Get(I)) + 2);
  Result:=AllocMem(Size+1);
  P := Result;
  for I := 0 to Count - 1 do
  begin
    S := Get(I);
    L := strlen(S);
    if L <> 0 then
    begin
      System.Move(Pointer(S)^, P^, L);
      Inc(P, L);
    end;
    P^ := #13;
    Inc(P);
    P^ := #10;
    Inc(P);
  end;
end;




function TPList.GetTextSize:integer;
var
  I,  Count: Integer;

begin
  Count := FCount;
  Result := 0;
  for I := 0 to Count - 1 do Inc(Result, strlen(Get(I)) + 2);
end;


procedure TPList.SetText(const Value: PChar);
var
  P, Start: PChar;
  S: string;
begin
  try
    Clear;
    P := Value;
    if P <> nil then
      while P^ <> #0 do
      begin
        Start := P;
        while not (P^ in [#0, #10, #13]) do Inc(P);
        SetString(S, Start, P - Start);
        Add(PChar(S));
        if P^ = #13 then Inc(P);
        if P^ = #10 then Inc(P);
      end;
      finally
      end;
end;

function TPList.FindUTF8(const S: PChar; var Index: Integer): Boolean;
var
  L, H, I, C: Integer;
begin
  Result := False;
  Index:=-1;
  if S = nil then exit;//!!!!!!!!NEW!
  L := 0;
  H := FCount - 1;
  while L <= H do
  begin
    I := (L + H) shr 1;
//    C := StrComp(FList^[I].FItem, S);
    C := keycmpUtf8(FList^[I].FItem,S,strlen(FList^[I].FItem),strlen(S));
    if C < 0 then L := I + 1 else
    begin
      H := I - 1;
      if C = 0 then
      begin
        Result := True;
        L := I;//��������� �� �������������!!!
//        if Duplicates <> dupAccept then L := I;
      end;
    end;
  end;
  Index := L;
end;


function TPList.Find(const S: PChar; var Index: Integer): Boolean;
var
  L, H, I, C: Integer;
begin
  Result := False;
  Index:=-1;
  if S = nil then exit;//!!!!!!!!NEW!
  L := 0;
  H := FCount - 1;
  while L <= H do
  begin
    I := (L + H) shr 1;
    C := StrComp(FList^[I].FItem, S);
    if C < 0 then L := I + 1 else
    begin
      H := I - 1;
      if C = 0 then
      begin
        Result := True;
        L := I;//��������� �� �������������!!!
//        if Duplicates <> dupAccept then L := I;
      end;
    end;
  end;
  Index := L;
end;


end.
