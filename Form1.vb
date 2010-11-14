'This application takes advantage of the application known as 
'Syring' Developed by the Chronic-Dev Team & Joshia Hill
'Release Notes below
'
'/**
'  * GreenPois0n Syringe
'  * Copyright (C) 2010 Chronic-Dev Team
'  * Copyright (C) 2010 Joshua Hill
'  *
'  * This program is free software: you can redistribute it and/or modify
'  * it under the terms of the GNU General Public License as published by
'  * the Free Software Foundation, either version 3 of the License, or
'  * (at your option) any later version.
'  *
'  * This program is distributed in the hope that it will be useful,
'  * but WITHOUT ANY WARRANTY; without even the implied warranty of
' * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' * GNU General Public License for more details.
'  *
'  * You should have received a copy of the GNU General Public License
'  * along with this program.  If not, see <http://www.gnu.org/licenses/&gt;.
' **/
'

Imports MobileDevice
Imports System
Imports System.IO
Imports System.Management
Imports System.Windows.Forms
Imports System.Security.Cryptography
Imports System.Text
Public Class Form1
#Region " ClientAreaMove Handling "
    Const WM_NCHITTEST As Integer = &H84
    Const HTCLIENT As Integer = &H1
    Const HTCAPTION As Integer = &H2

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Select Case m.Msg
            Case WM_NCHITTEST
                MyBase.WndProc(m)
                If m.Result = HTCLIENT Then m.Result = HTCAPTION
                'If m.Result.ToInt32 = HTCLIENT Then m.Result = IntPtr.op_Explicit(HTCAPTION) 'Try this in VS.NET 2002/2003 if the latter line of code doesn't do it... thx to Suhas for the tip.
            Case Else
                'Make sure you pass unhandled messages back to the default message handler.
                MyBase.WndProc(m)
        End Select
    End Sub
#End Region
    Public iDevice As String
    Public DFUConnected As Boolean
    Dim WithEvents iPhoneInterface As New MobileDevice.iPhone
    Public Event iPhoneConnected()
    Public Event iPhoneDisconnected()
    Public UploadediBSS As Boolean = False
    Public UploadediBEC As Boolean = False
    Public UploadedWTF As Boolean = False
    Public Downloaded As Boolean = False
    Public temppath As String = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\Temp\sn0wbreeze-iREB\"
    Public cmdline As String
    Private Sub iPhoneInterface_DfuConnect(ByVal sender As Object, ByVal e As System.EventArgs) Handles iPhoneInterface.DfuConnect
        Delay(1)
        DFUConnected = True
        If UploadedWTF = True Then
            UploadedWTF = False
            Label11.Invoke(CType(AddressOf Upload_iBSS, MethodInvoker))
        End If
    End Sub
    Private Sub iPhoneInterface_DfuDisconnect(ByVal sender As Object, ByVal e As System.EventArgs) Handles iPhoneInterface.DfuDisconnect
        DFUConnected = False
    End Sub
    Private Sub iPhoneInterface_RecConnect(ByVal sender As Object, ByVal e As System.EventArgs) Handles iPhoneInterface.RecoveryModeEnter
        Delay(1)
        If UploadediBSS = True Then
            UploadediBSS = False
            Label11.Invoke(CType(AddressOf Exec_Exploit, MethodInvoker))
        End If

        If UploadediBEC = True Then
            UploadediBEC = False
            Delay(5)
            Label11.Invoke(CType(AddressOf GoGoGadget_iBEC, MethodInvoker))
        End If
    End Sub
    Private Sub iPhoneInterface_RecDisconnect(ByVal sender As Object, ByVal e As System.EventArgs) Handles iPhoneInterface.RecoveryModeLeave
        'Nothing
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim Kill1 As String
        Dim Kill2 As String
        Kill1 = "cmd /c taskkill /f /t /im iTunes.exe"
        Kill2 = "cmd /c taskkill /f /t /im iTunesHelper.exe"
        'Shutting Down iTunes...
        'Shell(Kill1)
        'Shell(Kill2)
        BackgroundWorker1.RunWorkerAsync()
    End Sub
    Public Sub Exec_Exploit()
        Label11.Text = "Pwning iBSS..."
        cmdline = Quote.Text & "itunnel.exe" & Quote.Text & " -exploit " & Quote.Text & temppath & "iBSS.payload" & Quote.Text
        ExecCmd(cmdline, True)
        If iDevice = "iPhone 2G" Then
            Label11.Text = "Uploading PWNED iBSS..."
            cmdline = Quote.Text & "itunnel.exe" & Quote.Text & " -recfile " & Quote.Text & temppath & "iBEC.m68ap.RELEASE.dfu" & Quote.Text
            ExecCmd(cmdline, True)
            cmdline = Quote.Text & "itunnel.exe" & Quote.Text & " -c " & Quote.Text & "go" & Quote.Text
            ExecCmd(cmdline, True)
            UploadediBEC = True
            Label11.Text = "Waiting for iBEC..."
        ElseIf iDevice = "iPhone 3G" Then
            Label11.Text = "Uploading PWNED iBSS..."
            cmdline = Quote.Text & "itunnel.exe" & Quote.Text & " -recfile " & Quote.Text & temppath & "iBSS.n82ap.RELEASE-proper.dfu" & Quote.Text
            ExecCmd(cmdline, True)
            cmdline = Quote.Text & "itunnel.exe" & Quote.Text & " -c " & Quote.Text & "go" & Quote.Text
            ExecCmd(cmdline, True)
            UploadediBEC = True
            Label11.Text = "Waiting for iBSS (again)..."
        ElseIf iDevice = "iPod Touch 1G" Then
            Label11.Text = "Uploading PWNED iBSS..."
            cmdline = Quote.Text & "itunnel.exe" & Quote.Text & " -recfile " & Quote.Text & temppath & "iBEC.n45ap.RELEASE.dfu" & Quote.Text
            ExecCmd(cmdline, True)
            cmdline = Quote.Text & "itunnel.exe" & Quote.Text & " -c " & Quote.Text & "go" & Quote.Text
            ExecCmd(cmdline, True)
            UploadediBEC = True
            Label11.Text = "Waiting for iBEC..."
        ElseIf iDevice = "iPod Touch 2G" Then
            Label11.Text = "Setting up iBSS..."
            ProgressBar1.Value = 90
            cmdline = Quote.Text & "itunnel.exe" & Quote.Text & " -setpic " & Quote.Text & temppath & "wait.img3" & Quote.Text
            ExecCmd(cmdline, True)
            ProgressBar1.Value = 100
            Label11.Text = "Done! :)"
            MsgBox("If you have the " & Quote.Text & "Waiting for Custom Firmware" & Quote.Text & " on your device." & Chr(13) & "You may restore to custom firmware." & Chr(13) & Chr(13) & "Note: If you get Error 2003 in iTunes after Extracting," & Chr(13) & "Just unplug/replug it in and do it again.", MsgBoxStyle.Information)
            Label11.Text = "Cleaning up..."
            'Delete
            SaveToDisk("cleanup.bat", "cleanup.bat")
            cmdline = "cmd /c " & Quote.Text & temppath & "cleanup.bat" & Quote.Text
            ExecCmd(cmdline, True)
            cmdline = "cmd /c DEL " & Quote.Text & temppath & "cleanup.bat" & Quote.Text & " /f /q"
            Application.Exit()
        End If
    End Sub
    Public Sub Upload_iBSS()
        If iDevice = "iPhone 2G" Then
            Label11.Text = "Uploading iBSS..."
            cmdline = Quote.Text & "itunnel.exe" & Quote.Text & " -dfufile " & Quote.Text & temppath & "iBSS.m68ap.RELEASE.dfu" & Quote.Text
            ExecCmd(cmdline, True)
            ProgressBar1.Value = 45
            Label11.Text = "Waiting for iBSS..."
            UploadediBSS = True
        ElseIf iDevice = "iPhone 3G" Then
            Label11.Text = "Uploading iBSS..."
            cmdline = Quote.Text & "itunnel.exe" & Quote.Text & " -dfufile " & Quote.Text & temppath & "iBSS.n82ap.RELEASE.dfu" & Quote.Text
            ExecCmd(cmdline, True)
            ProgressBar1.Value = 45
            Label11.Text = "Waiting for iBSS..."
            UploadediBSS = True
        ElseIf iDevice = "iPod Touch 1G" Then
            Label11.Text = "Uploading iBSS..."
            cmdline = Quote.Text & "itunnel.exe" & Quote.Text & " -dfufile " & Quote.Text & temppath & "iBSS.n45ap.RELEASE.dfu" & Quote.Text
            ExecCmd(cmdline, True)
            ProgressBar1.Value = 45
            Label11.Text = "Waiting for iBSS..."
            UploadediBSS = True
        ElseIf iDevice = "iPod Touch 2G" Then
            Label11.Text = "Uploading iBSS..."
            cmdline = Quote.Text & "itunnel.exe" & Quote.Text & " -dfufile " & Quote.Text & temppath & "iBSS.n72ap.RELEASE.dfu" & Quote.Text
            ExecCmd(cmdline, True)
            ProgressBar1.Value = 45
            Label11.Text = "Waiting for iBSS..."
            UploadediBSS = True
        End If

    End Sub
    Public Sub Upload_WTF()
        Label11.Text = "Uploading WTF..."
        cmdline = Quote.Text & "itunnel.exe" & Quote.Text & " -dfufile " & Quote.Text & temppath & "WTF.s5l8900xall.RELEASE.dfu" & Quote.Text
        ExecCmd(cmdline, True)
        ProgressBar1.Value = 40
        Delay(2)
        Label11.Text = "Uploading iBSS..."
        If iDevice = "iPhone 3G" Then
            cmdline = Quote.Text & "itunnel.exe" & Quote.Text & " -dfufile " & Quote.Text & temppath & "iBSS.n82ap.RELEASE.dfu" & Quote.Text
            ExecCmd(cmdline, True)
        ElseIf iDevice = "iPhone 2G" Then
            cmdline = Quote.Text & "itunnel.exe" & Quote.Text & " -dfufile " & Quote.Text & temppath & "iBSS.m68ap.RELEASE.dfu" & Quote.Text
            ExecCmd(cmdline, True)
        ElseIf iDevice = "iPod Touch 1G" Then
            cmdline = Quote.Text & "itunnel.exe" & Quote.Text & " -dfufile " & Quote.Text & temppath & "iBSS.n45ap.RELEASE.dfu" & Quote.Text
            ExecCmd(cmdline, True)
        End If
        ProgressBar1.Value = 45
        Label11.Text = "Waiting for iBSS..."
        UploadediBSS = True
    End Sub
    Public Sub GoGoGadget_iBEC()
        Delay(3)
        Label11.Text = "Setting Up..."
        ProgressBar1.Value = 90
        cmdline = Quote.Text & "itunnel.exe" & Quote.Text & " -c " & Quote.Text & "bgcolor 0 255 0" & Quote.Text
        ExecCmd(cmdline, True)
        ProgressBar1.Value = 100
        Label11.Text = "Done! :)"
        MsgBox("If you have a Green screen on your device." & Chr(13) & "You may restore to custom firmware." & Chr(13) & Chr(13) & "Note: If you get Error 2003 in iTunes after Extracting," & Chr(13) & "Just unplug/replug it in and do it again.", MsgBoxStyle.Information)
        Label11.Text = "Cleaning up..."
        'Delete
        SaveToDisk("cleanup.bat", "cleanup.bat")
        cmdline = "cmd /c " & Quote.Text & temppath & "cleanup.bat" & Quote.Text
        ExecCmd(cmdline, True)
        cmdline = "cmd /c DEL " & Quote.Text & temppath & "cleanup.bat" & Quote.Text & " /f /q"
        ExecCmd(cmdline, True)
        Application.Exit()
    End Sub
    Public Sub limera1n()
        Label11.Text = "Exploiting with limera1n..."
        cmdline = Quote.Text & "pois0n.exe" & Quote.Text & " run"
        ExecCmd(cmdline, True)
        Label11.Text = "Done! :)"
        MsgBox("Your device is now in a PWNED DFU state (black screen)." & Chr(13) & Chr(13) & "You may now launch iTunes and do SHIFT + Restore" & Chr(13) & "to the custom sn0wbreeze IPSW located on your desktop!" & Chr(13) & Chr(13) & "Note: If you get Error 2003 in iTunes after Extracting," & Chr(13) & "Just unplug/replug it in and do it again.", MsgBoxStyle.Information)
        Label11.Text = "Cleaning up..."
        'Delete
        SaveToDisk("cleanup.bat", "cleanup.bat")
        cmdline = "cmd /c " & Quote.Text & temppath & "cleanup.bat" & Quote.Text
        ExecCmd(cmdline, True)
        cmdline = "cmd /c DEL " & Quote.Text & temppath & "cleanup.bat" & Quote.Text & " /f /q"
        ExecCmd(cmdline, True)
        Application.Exit()
    End Sub
    Public Sub steaks4uce()
        Label11.Text = "Exploiting with steaks4uce..."
        cmdline = Quote.Text & "pois0n.exe" & Quote.Text & " run"
        ExecCmd(cmdline, True)
        Label11.Text = "Done! :)"
        MsgBox("Your device is now in a PWNED DFU state (black screen)." & Chr(13) & Chr(13) & "You may now launch iTunes and do SHIFT + Restore" & Chr(13) & "to the custom sn0wbreeze IPSW located on your desktop!" & Chr(13) & Chr(13) & "Note: If you get Error 2003 in iTunes after Extracting," & Chr(13) & "Just unplug/replug it in and do it again.", MsgBoxStyle.Information)
        Label11.Text = "Cleaning up..."
        'Delete
        SaveToDisk("cleanup.bat", "cleanup.bat")
        cmdline = "cmd /c " & Quote.Text & temppath & "cleanup.bat" & Quote.Text
        ExecCmd(cmdline, True)
        cmdline = "cmd /c DEL " & Quote.Text & temppath & "cleanup.bat" & Quote.Text & " /f /q"
        ExecCmd(cmdline, True)
        Application.Exit()
    End Sub
    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        If File_Exists(temppath & "\iPhone2G.True") = True Then
            iDevice = "iPhone 2G"
            Label11.Invoke(CType(AddressOf Upload_WTF, MethodInvoker))
        ElseIf File_Exists(temppath & "\iPhone3G.True") = True Then
            iDevice = "iPhone 3G"
            Label11.Invoke(CType(AddressOf Upload_WTF, MethodInvoker))
        ElseIf File_Exists(temppath & "\iPhone3GS.True") = True Then
            iDevice = "iPhone 3GS"
            Label11.Invoke(CType(AddressOf limera1n, MethodInvoker))
        ElseIf File_Exists(temppath & "\iPhone4.True") = True Then
            iDevice = "iPhone 4"
            Label11.Invoke(CType(AddressOf limera1n, MethodInvoker))
        ElseIf File_Exists(temppath & "\iPad.True") = True Then
            iDevice = "iPad"
            Label11.Invoke(CType(AddressOf limera1n, MethodInvoker))
        ElseIf File_Exists(temppath & "\AppleTV2.True") = True Then
            iDevice = "Apple TV 2"
            Label11.Invoke(CType(AddressOf limera1n, MethodInvoker))
        ElseIf File_Exists(temppath & "\iPod1G.True") = True Then
            iDevice = "iPod Touch 1G"
            Label11.Invoke(CType(AddressOf Upload_WTF, MethodInvoker))
        ElseIf File_Exists(temppath & "\iPod2G.True") = True Then
            iDevice = "iPod Touch 2G"
            Label11.Invoke(CType(AddressOf steaks4uce, MethodInvoker))
        ElseIf File_Exists(temppath & "\iPod3G.True") = True Then
            iDevice = "iPod Touch 3G"
            Label11.Invoke(CType(AddressOf limera1n, MethodInvoker))
        ElseIf File_Exists(temppath & "\iPod4.True") = True Then
            iDevice = "iPod Touch 4"
            Label11.Invoke(CType(AddressOf limera1n, MethodInvoker))

        End If
    End Sub
End Class
