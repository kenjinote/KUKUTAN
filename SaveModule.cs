using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KUKUTAN
{
    class SaveModule
    {
    }
}
/*
Option Strict Off
Option Explicit On
Module SaveModule
	Declare Function CopyFile Lib "kernel32"  Alias "CopyFileA"(ByVal lpExistingFileName As String, ByVal lpNewFileName As String, ByVal bFailIfExists As Integer) As Integer
	
	Public Const ID_MAX As Short = 1100 '登録者数（最大登録可能ID)
	Public Const ID_MIN As Short = 1001
	Public Const FILE_SIZE As Short = 1000 'ファイル当り登録者数
	Public Const ID_LEN As Short = 4 'IDの最大桁数
	
	Public P_DATA(9999) As PRIVATE_DATA '個人成績用
	Public S_DATA(ID_MAX - 1) As NAMAE_DATA '名前用
	Public H_DATA(9999) As PRIVATE_DATA 'USBに保存済みの個人成績と比較用
	
	Public cnt As Short '登録者数
	Public h_cnt As Short '登録者数
	Public highscore As Short
	Public flag As Short
	
	
	'ファイルパス・ファイル名
	'ユーザプロファイルへのパスは環境変数から取得する Environ("USERPROFILE")
	'TopPageロード時に、フォルダが存在しているかチェックし、無ければ作成する
	Public Const PATH_P As String = "\Soromon"
	Public Const PATH_C As String = "\kukutan"
	Public Const PATH_D As String = "\seiseki"
	Public Const PATH_E As String = "\namaetouroku"
	Public PATH_ID As String
	
	Public Const PRIVATE_FILE As String = "\Pri_fla"
	Public Const BACKUP_FILE As String = "\BuckUp"
	Public Const TMP_FILE As String = "\temp"
	Public Const EXTENTION As String = ".csv"
	
	
	Public touroku_num As Short '登録ID
	
	Declare Function sndPlaySound Lib "winmm.dll"  Alias "sndPlaySoundA"(ByVal lpszSoundName As String, ByVal uFlags As Integer) As Integer 'VBのバージョンが4.0以上の時
	
	
	Structure PRIVATE_DATA 'ファイルの中のデータ
		Dim snai As Short 'チャレンジした問題番号
		Dim smon As Short '問題数
		Dim ssei As Short '正解数
		Dim shi As String 'チャレンジした時の日付
		Dim sjika As String 'チャレンジした時の時間
	End Structure
	
	Structure NAMAE_DATA 'ファイルの中のデータ
		Dim namae As String '名前
	End Structure
	
	Public Sub CheckFolderFile()
		Dim a As Object
		Dim i As Object
		Dim Fso As Object '必要なフォルダーが存在するか確認する

        Fso = CreateObject("Scripting.FileSystemObject")
		'UPGRADE_WARNING: オブジェクト Fso.FolderExists の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If Not Fso.FolderExists(Environ("USERPROFILE") & PATH_P) Then
			'UPGRADE_WARNING: オブジェクト Fso.CreateFolder の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			Fso.CreateFolder(Environ("USERPROFILE") & PATH_P)
		End If
		
		'20160808 追加
		'UPGRADE_WARNING: オブジェクト Fso.FolderExists の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If Not Fso.FolderExists(Environ("USERPROFILE") & PATH_P & PATH_E) Then
			'UPGRADE_WARNING: オブジェクト Fso.CreateFolder の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			Fso.CreateFolder(Environ("USERPROFILE") & PATH_P & PATH_E)
		End If
		
		'UPGRADE_WARNING: オブジェクト Fso.FolderExists の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If Not Fso.FolderExists(Environ("USERPROFILE") & PATH_P & PATH_C) Then
			'UPGRADE_WARNING: オブジェクト Fso.CreateFolder の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			Fso.CreateFolder(Environ("USERPROFILE") & PATH_P & PATH_C)
		End If
		
		'UPGRADE_WARNING: オブジェクト Fso.FolderExists の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If Not Fso.FolderExists(Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D) Then
			'UPGRADE_WARNING: オブジェクト Fso.CreateFolder の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			Fso.CreateFolder(Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D)
		End If
		
		For i = ID_MIN To ID_MAX
			'UPGRADE_WARNING: オブジェクト i の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			PATH_ID = "\" & i
			'UPGRADE_WARNING: オブジェクト Fso.FolderExists の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If Not Fso.FolderExists(Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID) Then
				'UPGRADE_WARNING: オブジェクト Fso.CreateFolder の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Fso.CreateFolder(Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID)
			End If
			
			
			'20160808 追加
			'UPGRADE_WARNING: オブジェクト Fso.FileExists の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If Not Fso.FileExists(Environ("USERPROFILE") & PATH_P & PATH_E & PATH_ID & EXTENTION) Then
				'UPGRADE_WARNING: オブジェクト Fso.CreateTextFile の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				a = Fso.CreateTextFile(Environ("USERPROFILE") & PATH_P & PATH_E & PATH_ID & EXTENTION, False)
				'UPGRADE_WARNING: オブジェクト a.Close の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				a.Close()
			End If
			
			''''''''''''''''''''''''''''''''''''''''''''''''''''CSVファイルを作成（成績保存・進級具合）
			'合格（進級）状況を保存するファイル
			'UPGRADE_WARNING: オブジェクト Fso.FileExists の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If Not Fso.FileExists(Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & "\" & "Pass_situation" & EXTENTION) Then
				'UPGRADE_WARNING: オブジェクト Fso.CreateTextFile の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				a = Fso.CreateTextFile(Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & "\" & "Pass_situation" & EXTENTION, False)
				'UPGRADE_WARNING: オブジェクト a.Close の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				a.Close()
			End If
			'やった問題を全て記録するファイル
			'UPGRADE_WARNING: オブジェクト Fso.FileExists の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If Not Fso.FileExists(Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & "\" & "the_history" & EXTENTION) Then
				'UPGRADE_WARNING: オブジェクト Fso.CreateTextFile の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				a = Fso.CreateTextFile(Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & "\" & "the_history" & EXTENTION, False)
				'UPGRADE_WARNING: オブジェクト a.Close の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				a.Close()
			End If
			'間違えた問題を全て記憶するファイル
			'UPGRADE_WARNING: オブジェクト Fso.FileExists の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If Not Fso.FileExists(Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & "\" & "mistake_history" & EXTENTION) Then
				'UPGRADE_WARNING: オブジェクト Fso.CreateTextFile の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				a = Fso.CreateTextFile(Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & "\" & "mistake_history" & EXTENTION, False)
				'UPGRADE_WARNING: オブジェクト a.Close の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				a.Close()
			End If
		Next i
	End Sub
	Public Sub zidoukakunin() '自動でやる段・問題表示形式・じゅんじょを設定する
        Dim s As Short
        Dim gouhi As String
		Dim fileNum As Short
		
		
		Dim Fso As New Scripting.FileSystemObject
		Dim FsoTS As Scripting.TextStream
		
		
		PATH_ID = "\" & touroku_num
		FsoTS = Fso.OpenTextFile(Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & "\Pass_situation" & EXTENTION, Scripting.IOMode.ForAppending) '合格履歴を調べる
		If FsoTS.Line = 1 Then 'ファイルが一行の場合は初挑戦として、クリアしていないというデータを書き込み
			
			For s = 1 To 184
				FsoTS.WriteLine("0")
			Next 
			dan = 1
			keishiki = 0
			zyunzyo = 0
			FsoTS.Close()
			'UPGRADE_NOTE: オブジェクト FsoTS をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
			FsoTS = Nothing
			Exit Sub
			
		Else 'ファイルが一行でない場合は段ごとにクリアしているか確認してクリアしていない場合はそこから挑戦する
			FsoTS.Close()
			'UPGRADE_NOTE: オブジェクト FsoTS をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
			FsoTS = Nothing
			fileNum = FreeFile
			s = 0
			FileOpen(fileNum, Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & "\Pass_situation" & EXTENTION, OpenMode.Input)
			
			Do Until EOF(fileNum)
				gouhi = LineInput(fileNum)
				s = s + 1
				If gouhi = "0" Then
					If s < 20 Then
						dan = 1
						If s = 1 Then keishiki = 0 : zyunzyo = 0
						If s = 2 Then keishiki = 0 : zyunzyo = 1
						If s = 3 Then keishiki = 0 : zyunzyo = 2
						If s = 4 Then keishiki = 1 : zyunzyo = 0 : sokudo = 0
						If s = 5 Then keishiki = 1 : zyunzyo = 1 : sokudo = 0
						If s = 6 Then keishiki = 1 : zyunzyo = 2 : sokudo = 0
						If s = 7 Then keishiki = 1 : zyunzyo = 0 : sokudo = 1
						If s = 8 Then keishiki = 1 : zyunzyo = 1 : sokudo = 1
						If s = 9 Then keishiki = 1 : zyunzyo = 2 : sokudo = 1
						If s = 10 Then keishiki = 1 : zyunzyo = 0 : sokudo = 2
						If s = 11 Then keishiki = 1 : zyunzyo = 1 : sokudo = 2
						If s = 12 Then keishiki = 1 : zyunzyo = 2 : sokudo = 2
						If s = 13 Then keishiki = 2 : zyunzyo = 0
						If s = 14 Then keishiki = 2 : zyunzyo = 1
						If s = 15 Then keishiki = 2 : zyunzyo = 2
						If s = 16 Then keishiki = 3 : zyunzyo = 0
						If s = 17 Then keishiki = 3 : zyunzyo = 1
						If s = 18 Then keishiki = 3 : zyunzyo = 2
						If s = 19 Then keishiki = 4 : zyunzyo = 2
						
					ElseIf s >= 20 And s < 172 Then 
						dan = ((s - 1) \ 19) + 1
						s = s - ((dan - 1) * 19)
						If s = 1 Then keishiki = 0 : zyunzyo = 0
						If s = 2 Then keishiki = 0 : zyunzyo = 1
						If s = 3 Then keishiki = 0 : zyunzyo = 2
						If s = 4 Then keishiki = 1 : zyunzyo = 0 : sokudo = 0
						If s = 5 Then keishiki = 1 : zyunzyo = 1 : sokudo = 0
						If s = 6 Then keishiki = 1 : zyunzyo = 2 : sokudo = 0
						If s = 7 Then keishiki = 1 : zyunzyo = 0 : sokudo = 1
						If s = 8 Then keishiki = 1 : zyunzyo = 1 : sokudo = 1
						If s = 9 Then keishiki = 1 : zyunzyo = 2 : sokudo = 1
						If s = 10 Then keishiki = 1 : zyunzyo = 0 : sokudo = 2
						If s = 11 Then keishiki = 1 : zyunzyo = 1 : sokudo = 2
						If s = 12 Then keishiki = 1 : zyunzyo = 2 : sokudo = 2
						If s = 13 Then keishiki = 2 : zyunzyo = 0
						If s = 14 Then keishiki = 2 : zyunzyo = 1
						If s = 15 Then keishiki = 2 : zyunzyo = 2
						If s = 16 Then keishiki = 3 : zyunzyo = 0
						If s = 17 Then keishiki = 3 : zyunzyo = 1
						If s = 18 Then keishiki = 3 : zyunzyo = 2
						If s = 19 Then keishiki = 4 : zyunzyo = 2
					ElseIf s >= 172 And s < 175 Then 
						dan = 10
						zyunzyo = s - 172
						keishiki = 5
					Else
						dan = s - 174
						keishiki = 6
						zyunzyo = 2
						
					End If
					FileClose(fileNum)
					Exit Sub
				End If
			Loop 
			FileClose(fileNum)
		End If
		
		dan = 10
		keishiki = 5
		zyunzyo = 2
		
	End Sub
	Public Sub sinkyukakunin()
		Dim intFileNo As Short 'ファイルNo
		Dim TextLine As String
		Dim CellsData As Object
		Dim byou(10) As Short
		Dim mondaisu(10) As Short
        Dim t As Object
        Dim s As Integer
		Dim syoudandata(184) As String
		Dim hensudai As Short
		Dim yomikomi As String
		PATH_ID = "\" & touroku_num
		
		intFileNo = FreeFile
		FileOpen(intFileNo, CurDir() & "\settei.csv", OpenMode.Input)
		
		'UPGRADE_WARNING: オブジェクト t の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		t = 0
		s = 1
		Do Until EOF(intFileNo) 'EOF(intFileNo)が True になるまで実行
			TextLine = LineInput(intFileNo) '1行全体を変数に読み込む
			TextLine = Replace(TextLine, Chr(34), "") ' "" を取り除く
			'UPGRADE_WARNING: オブジェクト CellsData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			CellsData = Split(TextLine, ",") 'カンマ区切りで列データを分割
			
			'UPGRADE_WARNING: オブジェクト t の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			t = t + 1
			'UPGRADE_WARNING: オブジェクト t の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト CellsData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			byou(t) = CellsData(1)
			'UPGRADE_WARNING: オブジェクト t の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト CellsData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			mondaisu(t) = CellsData(0)
		Loop 
		FileClose(intFileNo)
		
		
		If mondaisu(hensudai) <= collect_count Then '正解数が問題数より多かった場合は合格にする
			
			intFileNo = FreeFile
			FileOpen(intFileNo, Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & "\Pass_situation" & EXTENTION, OpenMode.Input)
			'UPGRADE_WARNING: オブジェクト t の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			t = 0
			
			Do Until EOF(intFileNo) 'EOF(intFileNo)が True になるまで実行
				yomikomi = LineInput(intFileNo)
				'UPGRADE_WARNING: オブジェクト t の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				syoudandata(t + 1) = yomikomi '1行全体を変数に読み込む
				'UPGRADE_WARNING: オブジェクト t の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				t = t + 1
			Loop 
			FileClose(intFileNo)
			
			If keishiki = 0 Then
				syoudandata((dan - 1) * 19 + (keishiki * 3) + 1 + zyunzyo) = CStr(1) '1-3
			ElseIf keishiki = 1 Then 
				If sokudo = 0 Then
					syoudandata((dan - 1) * 19 + (keishiki * 3) + 1 + zyunzyo + sokudo) = CStr(1) '4-12
				ElseIf sokudo = 1 Then 
					syoudandata((dan - 1) * 19 + (keishiki * 3) + 1 + zyunzyo + (sokudo * 3)) = CStr(1) '4-12
				ElseIf sokudo = 2 Then 
					syoudandata((dan - 1) * 19 + (keishiki * 3) + 1 + zyunzyo + (sokudo * 3)) = CStr(1) '4-12
				End If
			ElseIf keishiki = 2 Or keishiki = 3 Then 
				syoudandata((dan - 1) * 19 + (keishiki * 3) + 1 + zyunzyo + 6) = CStr(1) '13-18
			ElseIf keishiki = 4 Then 
				syoudandata((dan - 1) * 19 + (keishiki * 3) + 1 + 6) = CStr(1) '19
			ElseIf keishiki = 5 Then 
				syoudandata(172 + zyunzyo) = CStr(1) '20・21・22
			ElseIf keishiki = 6 Then 
				syoudandata(174 + dan) = CStr(1) '20・21・22
			End If
			
			
			
			intFileNo = FreeFile
			FileOpen(intFileNo, Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & "\Pass_situation" & EXTENTION, OpenMode.Output)
			For t = 0 To 183
				'UPGRADE_WARNING: オブジェクト t の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				PrintLine(intFileNo, syoudandata(t + 1))
			Next 
			
			FileClose(intFileNo)
			
		End If
		
	End Sub
	
	Public Sub mistakekakunin()
		Dim t As Object
		Dim i As Object
		
		'*******成績ファイルの作成***********
		Dim fileNum As Short

        PATH_ID = "\" & touroku_num
		
		fileNum = FreeFile
		
		If keishiki <> 5 Then
			FileOpen(fileNum, Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & "\mistake_history" & EXTENTION, OpenMode.Append)
			For i = 1 To 9
				For t = 1 To 9
					'UPGRADE_WARNING: オブジェクト t の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト i の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If huseikaidankakunin(i, t) = True Then
						WriteLine(fileNum, i, t, VB6.Format(Now, "ddddd"), VB6.Format(Now, "ttttt"))
					End If
				Next 
			Next 
			FileClose(fileNum)
		End If
		
		
		
		
		
	End Sub
	
	
	Public Sub InputSeisekiFile()
		'    Dim i As Integer
		'    Dim rensyutipe As String
		'    Dim daimoku(5) As String
		'    Dim fileNum As Integer
		'
		'PATH_ID = "\" & touroku_num
		'
		'    i = 0
		'    indata = 0
		'    cnt = 0
		'
		'*****ファイル読込み****
		'    fileNum = FreeFile
		'
		'    If keishiki <> 5 And keishiki <> 7 And keishiki <> 8 Then
		'            Open Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & "\" & dan & "dan-" & keishiki & "-" & zyunzyo & EXTENTION For Input As fileNum
		'    ElseIf keishiki = 5 Then
		'        Open Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & rensyutipe & zyunzyo + 1 & EXTENTION For Input As fileNum
		'    End If
		'
		'    cnt = 1
		'    Close fileNum
		
	End Sub
	
	Public Sub MakeSeisekiFile()
		
		'*******成績ファイルの作成***********
		Dim fileNum As Short

        PATH_ID = "\" & touroku_num
		
		fileNum = FreeFile
		
		
		FileOpen(fileNum, Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & "\the_history" & EXTENTION, OpenMode.Append)
		WriteLine(fileNum, P_DATA(cnt).snai, P_DATA(cnt).smon, P_DATA(cnt).ssei, P_DATA(cnt).shi, P_DATA(cnt).sjika)
		FileClose(fileNum)
		
	End Sub
	
	Public Sub DeleteSeisekiFile()
		Dim a As Object
		Dim cFso As Object
		
		'FileSystemObjectインスタンスを生成します
		cFso = CreateObject("Scripting.FileSystemObject")
		
		'ファイルを削除します
		'UPGRADE_WARNING: オブジェクト cFso.DeleteFile の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		cFso.DeleteFile(Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & PRIVATE_FILE & EXTENTION)
		
		'ファイルを作成
		'UPGRADE_WARNING: オブジェクト cFso.FileExists の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If Not cFso.FileExists(Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & PRIVATE_FILE & EXTENTION) Then
			'UPGRADE_WARNING: オブジェクト cFso.CreateTextFile の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			a = cFso.CreateTextFile(Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & PRIVATE_FILE & EXTENTION, False)
			'UPGRADE_WARNING: オブジェクト a.Close の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			a.Close()
		End If
		
		'オブジェクトの解放
		'UPGRADE_NOTE: オブジェクト cFso をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cFso = Nothing
		
		cnt = 0
		
	End Sub
	
	Public Sub InputNamaeFile()
		Dim i As Short
		i = 0
		'*****ファイル読込み****
		Dim fileNum As Short
		fileNum = FreeFile
		
		FileOpen(fileNum, Environ("USERPROFILE") & PATH_P & PATH_E & PATH_ID & EXTENTION, OpenMode.Input)
		
		Do Until EOF(fileNum)
			Input(fileNum, S_DATA(0).namae)
		Loop 
		
		FileClose(fileNum)
	End Sub
	Public Sub MakeNamaeFile()
		'*******成績ファイルの作成***********
		Dim fileNum As Short
		fileNum = FreeFile
		FileOpen(fileNum, Environ("USERPROFILE") & PATH_P & PATH_E & PATH_ID & EXTENTION, OpenMode.Output)
		
		WriteLine(fileNum, S_DATA(0).namae)
		
		FileClose(fileNum)
	End Sub
	Public Sub DeleteNamaeFile()
		Dim a As Object
		Dim cFso As Object
		
		'FileSystemObjectインスタンスを生成します
		cFso = CreateObject("Scripting.FileSystemObject")
		
		'ファイルを削除します
		'UPGRADE_WARNING: オブジェクト cFso.DeleteFile の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		cFso.DeleteFile(Environ("USERPROFILE") & PATH_P & PATH_E & PATH_ID & EXTENTION)
		
		'ファイルを作成
		'UPGRADE_WARNING: オブジェクト cFso.FileExists の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If Not cFso.FileExists(Environ("USERPROFILE") & PATH_P & PATH_E & PATH_ID & EXTENTION) Then
			'UPGRADE_WARNING: オブジェクト cFso.CreateTextFile の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			a = cFso.CreateTextFile(Environ("USERPROFILE") & PATH_P & PATH_E & PATH_ID & EXTENTION, False)
			'UPGRADE_WARNING: オブジェクト a.Close の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			a.Close()
		End If
		
		'オブジェクトの解放
		'UPGRADE_NOTE: オブジェクト cFso をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cFso = Nothing
		
		cnt = 0
		
	End Sub
	
	Public Sub dataoutput()
		Dim q As Object
		Dim s As Object
		Dim a As Object
		Dim Fso As Object 'デスクトップに成績データを作成する
		Dim DesktopPath As Object
        Dim sakuseifoldaname As String = ""
        Dim i As Object
		Dim t As Short
		Dim seisekidata(16, 12) As String
		
		'デスクトップのパス取得(Shellの利用)
		'UPGRADE_WARNING: オブジェクト CreateObject().SpecialFolders の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト DesktopPath の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		DesktopPath = CreateObject("WScript.Shell").SpecialFolders("Desktop")
		
		
		Fso = CreateObject("Scripting.FileSystemObject")
		'UPGRADE_WARNING: オブジェクト DesktopPath の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト Fso.FolderExists の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If Not Fso.FolderExists(DesktopPath & "\九九タン成績データ") Then 'デスクトップに九九タンのフォルダが存在するか確認してなければ作成する
			'UPGRADE_WARNING: オブジェクト DesktopPath の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト Fso.CreateFolder の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			Fso.CreateFolder(DesktopPath & "\九九タン成績データ")
		End If
		
		'UPGRADE_WARNING: オブジェクト i の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		i = 0
		Do 
			'UPGRADE_WARNING: オブジェクト i の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			i = i + 1
			'UPGRADE_WARNING: オブジェクト i の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト DesktopPath の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト Fso.FolderExists の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If Not Fso.FolderExists(DesktopPath & "\九九タン成績データ\" & VB6.Format(Today, "yyyy年mm月dd日") & i) Then 'デスクトップに九九タンのフォルダが存在するか確認してなければ作成する
				'UPGRADE_WARNING: オブジェクト i の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				sakuseifoldaname = VB6.Format(Today, "yyyy年mm月dd日") & i
				'UPGRADE_WARNING: オブジェクト DesktopPath の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト Fso.CreateFolder の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Fso.CreateFolder(DesktopPath & "\九九タン成績データ\" & sakuseifoldaname)
				Exit Do
			End If
			'UPGRADE_WARNING: オブジェクト i の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト DesktopPath の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト Fso.FolderExists の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Loop Until Not Fso.FolderExists(DesktopPath & "\九九タン成績データ\" & VB6.Format(Today, "yyyy年mm月dd日") & i)
		
		
		seisekidata(0, 0) = "問題形式"
		seisekidata(0, 1) = "じゅんじょ"
		For i = 1 To 9
			'UPGRADE_WARNING: オブジェクト i の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			seisekidata(0, 1 + i) = i & "の段"
		Next 
		seisekidata(0, 11) = "ランダム"
		
		seisekidata(1, 0) = "もじ＋すうじ"
		seisekidata(2, 0) = "もじ＋すうじ"
		seisekidata(3, 0) = "もじ＋すうじ"
		seisekidata(4, 0) = "もじのみ"
		seisekidata(5, 0) = "もじのみ"
		seisekidata(6, 0) = "もじのみ"
		seisekidata(7, 0) = "すうじのみ"
		seisekidata(8, 0) = "すうじのみ"
		seisekidata(9, 0) = "すうじのみ"
		seisekidata(10, 0) = "穴埋"
		seisekidata(11, 0) = "穴埋"
		seisekidata(12, 0) = "穴埋"
		seisekidata(13, 0) = "81問チャレンジ"
		seisekidata(14, 0) = "81問チャレンジ"
		seisekidata(15, 0) = "81問チャレンジ"
		
		For i = 0 To 4
			'UPGRADE_WARNING: オブジェクト i の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			seisekidata(i * 3 + 1, 1) = "じゅんじょ：じゅんばん"
			'UPGRADE_WARNING: オブジェクト i の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			seisekidata(i * 3 + 2, 1) = "じゅんじょ：ぎゃくじゅん"
			'UPGRADE_WARNING: オブジェクト i の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			seisekidata(i * 3 + 3, 1) = "じゅんじょ：ランダム"
		Next 
		
		For i = ID_MIN To ID_MAX
			'UPGRADE_WARNING: オブジェクト i の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト DesktopPath の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト Fso.CreateTextFile の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			a = Fso.CreateTextFile(DesktopPath & "\九九タン成績データ\" & sakuseifoldaname & "\" & i & EXTENTION, False)
			'UPGRADE_WARNING: オブジェクト a.Close の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			a.Close()
		Next 
		
		Dim fileNum As Short
		
		For i = ID_MIN To ID_MAX
			
			For t = 1 To 16
				For s = 2 To 12
					'UPGRADE_WARNING: オブジェクト s の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					seisekidata(t, s) = ""
				Next 
			Next 
			
			
			For t = 0 To 3
				For s = 0 To 2
					fileNum = FreeFile
					FileOpen(fileNum, Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & "\Pass_situation" & EXTENTION, OpenMode.Input)
					'UPGRADE_WARNING: オブジェクト q の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					q = 0
					Do Until EOF(fileNum) 'EOF(intfileNum)が True になるまで実行
						'UPGRADE_WARNING: オブジェクト q の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト s の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						seisekidata(3 * (t - 1) + (s - 1) + 1, q + 2) = LineInput(fileNum) '1行全体を変数に読み込む
						'UPGRADE_WARNING: オブジェクト q の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						q = q + 1
					Loop 
					FileClose(fileNum)
				Next 
			Next 
			
			
			fileNum = FreeFile
			FileOpen(fileNum, Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & "\Pass_situation" & EXTENTION, OpenMode.Input)
			'UPGRADE_WARNING: オブジェクト q の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			q = 0
			Do Until EOF(fileNum) 'EOF(intfileNum)が True になるまで実行
				'UPGRADE_WARNING: オブジェクト q の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				seisekidata(13 + q, 11) = LineInput(fileNum) '1行全体を変数に読み込む
				'UPGRADE_WARNING: オブジェクト q の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				q = q + 1
			Loop 
			FileClose(fileNum)
			
			fileNum = FreeFile
			
			For t = 1 To 15
				For s = 2 To 11
					'UPGRADE_WARNING: オブジェクト s の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If seisekidata(t, s) = "1" Then
						'UPGRADE_WARNING: オブジェクト s の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						seisekidata(t, s) = "合格"
					Else
						'UPGRADE_WARNING: オブジェクト s の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						seisekidata(t, s) = ""
					End If
					'UPGRADE_WARNING: オブジェクト s の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If t Mod 3 <> 0 And s = 11 And t < 13 Then
						'UPGRADE_WARNING: オブジェクト s の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						seisekidata(t, s) = "-"
					End If
					If t = 10 Or t = 11 Then
						'UPGRADE_WARNING: オブジェクト s の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						seisekidata(t, s) = "-"
					End If
					'UPGRADE_WARNING: オブジェクト s の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If s <> 11 Then
						If t = 13 Or t = 14 Or t = 15 Then
							'UPGRADE_WARNING: オブジェクト s の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							seisekidata(t, s) = "-"
						End If
					End If
				Next 
			Next 
			
			
			
			
			'UPGRADE_WARNING: オブジェクト i の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト DesktopPath の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			FileOpen(fileNum, DesktopPath & "\九九タン成績データ\" & sakuseifoldaname & "\" & i & EXTENTION, OpenMode.Output)
			For t = 0 To 15
				PrintLine(fileNum, seisekidata(t, 0) & "," & seisekidata(t, 1) & "," & seisekidata(t, 2) & "," & seisekidata(t, 3) & "," & seisekidata(t, 4) & "," & seisekidata(t, 5) & "," & seisekidata(t, 6) & "," & seisekidata(t, 7) & "," & seisekidata(t, 8) & "," & seisekidata(t, 9) & "," & seisekidata(t, 10) & "," & seisekidata(t, 11))
			Next 
			FileClose(fileNum)
			
		Next 
	End Sub
	
	Public Sub zituryokuhantei()
		Dim q As Object
		Dim intFileNo As Object
		Dim t As Object
		Dim i As Object '判定モード
		Dim danhuseika(184) As Boolean
		Dim syoudandata(184) As String
		
		'不正解のある段を確認する
		For i = 1 To 9
			'UPGRADE_WARNING: オブジェクト i の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			danhuseika(i) = False
			
			For t = 1 To 9
				'UPGRADE_WARNING: オブジェクト t の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト i の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If huseikaidankakunin(i, t) = True Then
					'UPGRADE_WARNING: オブジェクト i の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					danhuseika(i) = True
					
					'UPGRADE_WARNING: オブジェクト intFileNo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					intFileNo = FreeFile
					'UPGRADE_WARNING: オブジェクト intFileNo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					FileOpen(intFileNo, Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & "\Pass_situation" & EXTENTION, OpenMode.Output)
					For q = 0 To 2
						'UPGRADE_WARNING: オブジェクト intFileNo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						PrintLine(intFileNo, "0")
					Next 
					'UPGRADE_WARNING: オブジェクト intFileNo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					FileClose(intFileNo)
					Exit For
				End If
			Next 
			
		Next 
		
		If keishiki = 5 Then
			For i = 0 To 3
				For t = 1 To 3
					'UPGRADE_WARNING: オブジェクト intFileNo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					intFileNo = FreeFile
					'UPGRADE_WARNING: オブジェクト intFileNo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					FileOpen(intFileNo, Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & "\Pass_situation" & EXTENTION, OpenMode.Input)
					'UPGRADE_WARNING: オブジェクト q の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					q = 0
					'UPGRADE_WARNING: オブジェクト intFileNo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					Do Until EOF(intFileNo) 'EOF(intFileNo)が True になるまで実行
						'UPGRADE_WARNING: オブジェクト q の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト intFileNo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						syoudandata(q) = LineInput(intFileNo) '1行全体を変数に読み込む
						'UPGRADE_WARNING: オブジェクト q の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						q = q + 1
					Loop 
					'UPGRADE_WARNING: オブジェクト intFileNo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					FileClose(intFileNo)
					
					For q = 0 To 8 '段が不正解だった場合、合格を取り消す
						'UPGRADE_WARNING: オブジェクト q の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If danhuseika(q + 1) = True Then
							'UPGRADE_WARNING: オブジェクト q の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							syoudandata(q) = "0"
							syoudandata(9) = CStr(0)
						End If
					Next 
					
					'UPGRADE_WARNING: オブジェクト intFileNo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					intFileNo = FreeFile
					'UPGRADE_WARNING: オブジェクト intFileNo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					FileOpen(intFileNo, Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & "\Pass_situation" & EXTENTION, OpenMode.Output)
					For q = 0 To 9
						'UPGRADE_WARNING: オブジェクト q の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト intFileNo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						PrintLine(intFileNo, syoudandata(q))
					Next 
					'UPGRADE_WARNING: オブジェクト intFileNo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					FileClose(intFileNo)
				Next 
			Next 
		ElseIf keishiki = 0 Or keishiki = 1 Or keishiki = 2 Or keishiki = 3 Then 
			'UPGRADE_WARNING: オブジェクト i の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			i = keishiki
			For t = 0 To 2
				'UPGRADE_WARNING: オブジェクト intFileNo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				intFileNo = FreeFile
				'UPGRADE_WARNING: オブジェクト intFileNo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				FileOpen(intFileNo, Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & "\Pass_situation" & EXTENTION, OpenMode.Input)
				'UPGRADE_WARNING: オブジェクト q の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				q = 0
				'UPGRADE_WARNING: オブジェクト intFileNo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Do Until EOF(intFileNo) 'EOF(intFileNo)が True になるまで実行
					'UPGRADE_WARNING: オブジェクト q の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト intFileNo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					syoudandata(q) = LineInput(intFileNo) '1行全体を変数に読み込む
					'UPGRADE_WARNING: オブジェクト q の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					q = q + 1
				Loop 
				'UPGRADE_WARNING: オブジェクト intFileNo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				FileClose(intFileNo)
				
				For q = 0 To 8 '段が不正解だった場合、合格を取り消す
					'UPGRADE_WARNING: オブジェクト q の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If danhuseika(q + 1) = True Then
						'UPGRADE_WARNING: オブジェクト q の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						syoudandata(q) = "0"
						syoudandata(9) = CStr(0)
					End If
				Next 
				
				'UPGRADE_WARNING: オブジェクト intFileNo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				intFileNo = FreeFile
				'UPGRADE_WARNING: オブジェクト intFileNo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				FileOpen(intFileNo, Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & "\Pass_situation" & EXTENTION, OpenMode.Output)
				For q = 0 To 9
					'UPGRADE_WARNING: オブジェクト q の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト intFileNo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					PrintLine(intFileNo, syoudandata(q))
				Next 
				'UPGRADE_WARNING: オブジェクト intFileNo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				FileClose(intFileNo)
			Next 
		End If
		
		
	End Sub
	Public Sub hukusyu()
        Dim p As Object = Nothing
        Dim q As Object = Nothing
        Dim Fso As Object
		Dim i As Object
		Dim fileNum As Object 'ふくしゅうモード
		
		'UPGRADE_WARNING: オブジェクト fileNum の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		fileNum = FreeFile
		
		For i = 1 To 20
			'UPGRADE_WARNING: オブジェクト i の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト hukusyudatami(i) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			hukusyudatami(i) = 0
			'UPGRADE_WARNING: オブジェクト i の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			hukusyudatahou(i) = 0
		Next 
		
		syutsudaimondaisu = 0
		
		Fso = CreateObject("Scripting.FileSystemObject")
		'UPGRADE_WARNING: オブジェクト Fso.FileExists の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If Fso.FileExists(Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & "\mistake_history" & EXTENTION) Then
			'UPGRADE_WARNING: オブジェクト fileNum の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			FileOpen(fileNum, Environ("USERPROFILE") & PATH_P & PATH_C & PATH_D & PATH_ID & "\mistake_history" & EXTENTION, OpenMode.Input)
			'UPGRADE_WARNING: オブジェクト i の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			i = 0
			'UPGRADE_WARNING: オブジェクト i の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fileNum の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			Do Until EOF(fileNum) Or i = 20
				'UPGRADE_WARNING: オブジェクト i の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				i = i + 1
				syutsudaimondaisu = syutsudaimondaisu + 1
				'UPGRADE_WARNING: オブジェクト i の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fileNum の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Input(fileNum, hukusyudatami(i))
				Input(fileNum, hukusyudatahou(i))
				Input(fileNum, q)
				Input(fileNum, p)
			Loop 
			'UPGRADE_WARNING: オブジェクト fileNum の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			FileClose(fileNum)
		End If
		
	End Sub
End Module

*/
