
Imports LibVLCSharp.Shared
Imports System.Threading



Public Class Form1


    Dim _libVLC As LibVLC
    Dim _mp As MediaPlayer
    Dim media As Media
    Dim videoView1 As LibVLCSharp.WinForms.VideoView = New LibVLCSharp.WinForms.VideoView

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        End
    End Sub



    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load



        Dim appPath As String = Application.StartupPath()
        'Dim test = appPath & "\LibVlc\win-x86\"
        Core.Initialize(appPath & "\LibVlc\win-x86\")
        _libVLC = New LibVLC("enableDebugLogs: true")
        _mp = New MediaPlayer(_libVLC)

        videoView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        videoView1.BackColor = System.Drawing.Color.Black
        videoView1.Location = New System.Drawing.Point(0, 0)
        videoView1.MediaPlayer = Nothing
        videoView1.Name = "videoView1"

        videoView1.Size = New Size(Panel1.Width, Panel1.Height)

        'videoView1.Size = New System.Drawing.Size(903, 508)
        videoView1.TabIndex = 0
        videoView1.Text = "videoView1"
        videoView1.BackColor = Color.Black
        Panel1.Controls.Add(videoView1)

        _mp.EnableHardwareDecoding = True
        _mp.EnableKeyInput = False
        _mp.EnableMouseInput = False
        videoView1.MediaPlayer = _mp
        videoView1.BringToFront()


        'handlers not used here but i use them in my app
        AddHandler Me.videoView1.DoubleClick, AddressOf Me.videoView1_DoubleClick
        AddHandler Me.videoView1.MouseClick, AddressOf Me.videview1_MouseClick
        AddHandler Me.videoView1.MouseUp, AddressOf Me.videoView1_MouseUp


        AddHandler Me._mp.Playing, AddressOf Me._mp_Playing
        AddHandler Me._mp.EndReached, AddressOf Me._mp_EndReached
        AddHandler Me._mp.Stopped, AddressOf Me._mp_Stopped
        AddHandler Me._mp.EncounteredError, AddressOf Me._mp_error

    End Sub

    Private Sub videoView1_DoubleClick(sender As Object, e As System.Windows.Forms.MouseEventArgs)

    End Sub

    Private Sub videview1_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs)

    End Sub

    Private Sub videoView1_MouseUp(sender As Object, e As MouseEventArgs)

    End Sub

    Private Sub _mp_Playing(sender As Object, e As System.EventArgs)

    End Sub

    Private Sub _mp_EndReached(sender As Object, e As System.EventArgs)

    End Sub

    Private Sub _mp_Stopped(sender As Object, e As System.EventArgs)

    End Sub

    Private Sub _mp_error(sender As Object, e As System.EventArgs)

    End Sub

    Dim playerwatchdog As Thread
    Dim params() As String = {}
    Dim stoporder As Boolean

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If TextBox1.Text = "" Then Exit Sub

        'if i comment _mp.SetAdjustFloat(0, 0) i get "Access violation reading location 0x69B9723C" error and app crashes
        '_mp.SetAdjustFloat(0, 0)
        _mp.Stop()
        _mp.Play(New Media(_libVLC, New Uri(TextBox1.Text), params))


        '_mp.SetAdjustFloat(VideoAdjustOption.Enable, 1.0F)
        '_mp.SetAdjustFloat(VideoAdjustOption.Enable, True)
        _mp.SetAdjustFloat(VideoAdjustOption.Enable, 1)
        _mp.SetAdjustFloat(LibVLCSharp.Shared.VideoAdjustOption.Brightness, 1.08)
        _mp.SetAdjustFloat(LibVLCSharp.Shared.VideoAdjustOption.Saturation, 1.08)

        stoporder = False
        If Not playerwatchdog Is Nothing Then playerwatchdog.Abort()
        playerwatchdog = New Thread(AddressOf watchdog)
        playerwatchdog.Start()

    End Sub

    Private Sub watchdog()

        Dim pos As Long = 0L
        Thread.Sleep(7000)
        Dim cond1 As Boolean
        Dim cond2 As Boolean


        Do
            Me.Invoke(Sub()

                          If stoporder Then Exit Sub
                          cond1 = (Not _mp.State = VLCState.Playing) And (Not _mp.State = VLCState.Opening)
                          cond2 = (_mp.State = VLCState.Playing) And (pos = _mp.Time) And (Not _mp.Time <= 0L)

                          If (cond1 Or cond2) Then

                              '_mp.SetAdjustFloat(0, 0)
                              _mp.Stop()

                              _mp.Play(New Media(_libVLC, New Uri(TextBox1.Text), params))

                          End If

                          If Not _mp.Time = -1 Then
                              pos = _mp.Time
                          End If

                      End Sub)
            
            Thread.Sleep(7000)
        Loop






    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        stoporder = True
        If Not playerwatchdog Is Nothing Then playerwatchdog.Abort()

        'if i comment _mp.SetAdjustFloat(0, 0) i get "Access violation reading location 0x69B9723C" error and app crashes
        '_mp.SetAdjustFloat(0, 0)

        _mp.Stop()
        stoporder = True
    End Sub
End Class

