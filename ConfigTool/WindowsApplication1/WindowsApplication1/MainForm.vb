Imports System.IO
Imports System.Net

Public Class MainForm
    Dim computers As New List(Of Computer)
    Dim act_computer As Computer
    Dim filename As String
    Dim foldername As String
    Dim projectname As String

    Private Results As String
    Private Delegate Sub delUpdate()
    Private Finished As New delUpdate(AddressOf UpdateText)
    Private Delegate Sub buttonsUpdate()
    Private FinishedButtons As New buttonsUpdate(AddressOf EnableButtons)
    Dim button_click As Integer = 0
    Dim pf As New ProjectForm
    Dim P As Process
    Dim pathToSlave As String = "C:\StudentenprojektCAVE\AktuellesProjekt\"
    Dim CMDThread As Threading.Thread

    Private Function clearAllFields()
        txt_ipAddress.Text = ""
        txt_port.Text = ""
        txt_computerName.Text = ""
        chk_master.Checked = False
        list_rela.SelectedIndex = 0
        txt_relat_x.Text = ""
        txt_relat_y.Text = ""
        txt_relat_z.Text = ""
        list_cam.SelectedIndex = 0
        txt_cam_x.Text = ""
        txt_cam_y.Text = ""
        txt_cam_z.Text = ""
        list_splane.SelectedIndex = 0
        txt_splane_x.Text = ""
        txt_splane_y.Text = ""
        txt_splane_z.Text = ""
    End Function

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Me.grp_computerInfo.Text = Me.ListBox1.SelectedItem.ToString
        'clearAllFields()

        act_computer = computers.ElementAt(Me.ListBox1.SelectedIndex)

        txt_ipAddress.Text = act_computer.ip
        txt_computerName.Text = act_computer.name

        If TypeOf act_computer Is Master Then
            chk_master.Checked = True
            chk_master.Enabled = False
            grp_relation.Enabled = True
            grp_cam.Enabled = False

            txt_port.Enabled = True
            txt_port.Text = DirectCast(act_computer, Master).port
            list_rela.SelectedIndex = 0
        Else
            txt_port.Text = ""
            txt_port.Enabled = False
            chk_master.Checked = False
            chk_master.Enabled = False
            grp_relation.Enabled = False
            grp_cam.Enabled = True
            list_cam.SelectedIndex = 0

            If DirectCast(act_computer, Slave).camera.eye.Equals("left") Then
                combo_eye.SelectedIndex = 0
            Else
                combo_eye.SelectedIndex = 1
            End If
        End If

        grp_computerInfo.Enabled = True
        grp_splane.Enabled = True
        list_splane.SelectedIndex = 0

    End Sub

    Private Sub ÖffnenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ÖffnenToolStripMenuItem.Click
        If FolderBrowserDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            foldername = FolderBrowserDialog1.SelectedPath
            projectname = FolderBrowserDialog1.SelectedPath.Substring(FolderBrowserDialog1.SelectedPath.LastIndexOf("\") + 1)
            filename = FolderBrowserDialog1.SelectedPath + "\" + projectname + "_Data\StreamingAssets\node-config.xml"
            If File.Exists(filename) Then
                Dim fs As New FileStream(filename, FileMode.Open, FileAccess.Read)
                ListBox1.Items.Clear()

                Dim xmlReader As New XMLReader
                computers = xmlReader.GetAllComputers(fs)

                For Each computer In computers
                    ListBox1.Items.Add(computer.ToString)
                Next

                fs.Close()
                grp_computerList.Enabled = True
                btn_configSave.Enabled = True
                btn_startProject.Enabled = True
                btn_deployProject.Enabled = True
                btn_update.Enabled = True
                txt_projectname.Text = projectname
                ListBox1.SelectedIndex = 0
            Else
                MsgBox("Configuration file not found. Please choose correct directory")
            End If
        End If
    End Sub

    Private Sub list_rela_SelectedIndexChanged(sender As Object, e As EventArgs) Handles list_rela.SelectedIndexChanged

        Dim relation As Origin = DirectCast(act_computer, Master).origin

        If list_rela.SelectedItem.Equals("Position") Then
            txt_relat_x.Text = relation.position.x
            txt_relat_y.Text = relation.position.y
            txt_relat_z.Text = relation.position.z
        End If


    End Sub

    Private Sub list_splane_SelectedIndexChanged(sender As Object, e As EventArgs) Handles list_splane.SelectedIndexChanged


        Dim screenplane As ScreenPlane = act_computer.screenplane

        If list_splane.SelectedItem.Equals("Position") Then
            txt_splane_x.Enabled = True
            txt_splane_y.Enabled = True
            txt_splane_z.Enabled = True
            txt_scale.Enabled = False
            txt_splane_x.Text = screenplane.position.x
            txt_splane_y.Text = screenplane.position.y
            txt_splane_z.Text = screenplane.position.z
        ElseIf list_splane.SelectedItem.Equals("Rotation") Then
            txt_splane_x.Enabled = True
            txt_splane_y.Enabled = True
            txt_splane_z.Enabled = True
            txt_scale.Enabled = False
            txt_splane_x.Text = screenplane.rotation.x
            txt_splane_y.Text = screenplane.rotation.y
            txt_splane_z.Text = screenplane.rotation.z
        ElseIf list_splane.SelectedItem.Equals("Scale") Then
            txt_splane_x.Enabled = False
            txt_splane_y.Enabled = False
            txt_splane_z.Enabled = False
            txt_scale.Enabled = True
            txt_scale.Text = screenplane.scale.x
        End If
    End Sub

    Private Sub list_cam_SelectedIndexChanged(sender As Object, e As EventArgs) Handles list_cam.SelectedIndexChanged

        Dim camera As Camera = DirectCast(act_computer, Slave).camera

        If list_cam.SelectedItem.Equals("Rotation") Then
            txt_cam_x.Text = camera.rotation.x
            txt_cam_y.Text = camera.rotation.y
            txt_cam_z.Text = camera.rotation.z
        End If
    End Sub

    Private Sub BeendenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BeendenToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btn_configSave_Click(sender As Object, e As EventArgs) Handles btn_configSave.Click
        Dim xmlWriter As New XMLWriter
        xmlWriter.SetAllComputers(filename, computers)
        MsgBox("Configuration saved", MsgBoxStyle.OkOnly, "Configuration saved")
    End Sub

    Private Sub txt_ipAddress_TextChanged(sender As Object, e As EventArgs) Handles txt_ipAddress.TextChanged
        act_computer.ip = txt_ipAddress.Text

        Dim position_lb1 As Integer = ListBox1.SelectedIndex

        ListBox1.Items.Clear()
        For Each computer In computers
            ListBox1.Items.Add(computer.ToString)
        Next
        ListBox1.SelectedIndex = position_lb1
    End Sub

    Private Sub txt_port_TextChanged(sender As Object, e As EventArgs) Handles txt_port.TextChanged
        If TypeOf act_computer Is Master Then
            DirectCast(act_computer, Master).port = txt_port.Text
        End If
    End Sub

    Private Sub txt_computerName_TextChanged(sender As Object, e As EventArgs) Handles txt_computerName.TextChanged
        act_computer.name = txt_computerName.Text
    End Sub

    Private Sub txt_relat_x_TextChanged(sender As Object, e As EventArgs) Handles txt_relat_x.TextChanged

        If list_rela.SelectedItem.Equals("Position") Then
            DirectCast(act_computer, Master).origin.position.x = txt_relat_x.Text
        End If

    End Sub

    Private Sub txt_cam_x_TextChanged(sender As Object, e As EventArgs) Handles txt_cam_x.TextChanged
        DirectCast(act_computer, Slave).camera.rotation.x = txt_cam_x.Text
    End Sub

    Private Sub txt_splane_x_TextChanged(sender As Object, e As EventArgs) Handles txt_splane_x.TextChanged
        If list_splane.SelectedItem.Equals("Position") Then
            act_computer.screenplane.position.x = txt_splane_x.Text
        ElseIf list_splane.SelectedItem.Equals("Rotation") Then
            act_computer.screenplane.rotation.x = txt_splane_x.Text
        End If
    End Sub

    Private Sub txt_relat_y_TextChanged(sender As Object, e As EventArgs) Handles txt_relat_y.TextChanged
        If list_rela.SelectedItem.Equals("Position") Then
            DirectCast(act_computer, Master).origin.position.y = txt_relat_y.Text
        End If
    End Sub

    Private Sub txt_relat_z_TextChanged(sender As Object, e As EventArgs) Handles txt_relat_z.TextChanged
        If list_rela.SelectedItem.Equals("Position") Then
            DirectCast(act_computer, Master).origin.position.z = txt_relat_z.Text
        End If
    End Sub

    Private Sub txt_cam_y_TextChanged(sender As Object, e As EventArgs) Handles txt_cam_y.TextChanged
        DirectCast(act_computer, Slave).camera.rotation.y = txt_cam_y.Text
    End Sub

    Private Sub txt_cam_z_TextChanged(sender As Object, e As EventArgs) Handles txt_cam_z.TextChanged
        DirectCast(act_computer, Slave).camera.rotation.z = txt_cam_z.Text
    End Sub

    Private Sub txt_splane_y_TextChanged(sender As Object, e As EventArgs) Handles txt_splane_y.TextChanged
        If list_splane.SelectedItem.Equals("Position") Then
            act_computer.screenplane.position.y = txt_splane_y.Text
        ElseIf list_splane.SelectedItem.Equals("Rotation") Then
            act_computer.screenplane.rotation.y = txt_splane_y.Text
        End If
    End Sub

    Private Sub txt_splane_z_TextChanged(sender As Object, e As EventArgs) Handles txt_splane_z.TextChanged
        If list_splane.SelectedItem.Equals("Position") Then
            act_computer.screenplane.position.z = txt_splane_z.Text
        ElseIf list_splane.SelectedItem.Equals("Rotation") Then
            act_computer.screenplane.rotation.z = txt_splane_z.Text
        End If
    End Sub

    Private Sub combo_eye_SelectedIndexChanged(sender As Object, e As EventArgs) Handles combo_eye.SelectedIndexChanged
        If combo_eye.SelectedIndex.Equals(0) Then
            DirectCast(act_computer, Slave).camera.eye = "left"
        Else
            DirectCast(act_computer, Slave).camera.eye = "right"
        End If
    End Sub

    Private Sub grp_computerInfo_MouseHover(sender As Object, e As EventArgs) Handles grp_computerInfo.MouseHover
        txt_info.Text = "Configure settings of Computer"
    End Sub

    Private Sub grp_computerList_MouseHover(sender As Object, e As EventArgs) Handles grp_computerList.MouseHover
        txt_info.Text = "List of Computers to configure"
    End Sub

    Private Sub opencmd_start()
        Dim Pr As New Process
        P = Pr
        Pr.Close()
        P.StartInfo.CreateNoWindow = True
        P.StartInfo.UseShellExecute = False
        P.StartInfo.RedirectStandardInput = True
        P.StartInfo.RedirectStandardOutput = True
        P.StartInfo.RedirectStandardError = True
        P.StartInfo.FileName = ".\MASTER_StartUnity.bat"
        P.Start()
    End Sub

    Private Sub opencmd_deployupdate(update As Boolean)
        Dim Pr As New Process
        P = Pr
        Pr.Close()
        P.StartInfo.CreateNoWindow = True
        P.StartInfo.UseShellExecute = False
        P.StartInfo.RedirectStandardInput = True
        P.StartInfo.RedirectStandardOutput = True
        P.StartInfo.RedirectStandardError = True
        P.StartInfo.FileName = ".\ProjektVerteilen.bat"
        If update Then
            P.StartInfo.Arguments = foldername + " update"
        Else
            P.StartInfo.Arguments = foldername
        End If
        P.Start()
    End Sub

    Private Sub UpdateText()
        Try
            pf.txt_projectForm.AppendText(System.Environment.NewLine() & Results)
            If Results.Contains(">xcopy") Or Results.Contains(">RMDIR") Or Results.Contains(">copy") Or Results.Contains(">perl") Then
                pf.ProgressBar1.PerformStep()
            End If
            pf.txt_projectForm.ScrollToCaret()
        Catch ex As Exception
            MsgBox("Error. Process cancelled.", MsgBoxStyle.Critical, "Error")
            P.Close()
            Invoke(FinishedButtons)
            CMDThread.Abort()
        End Try
    End Sub

    Private Sub CMDConfig()
        While P.StandardOutput.EndOfStream = False
            Results = P.StandardOutput.ReadLine()
            Invoke(Finished)
        End While
        P.Close()
        Invoke(FinishedButtons)
        CMDThread.Abort()
    End Sub

    Private Sub EnableButtons()
        btn_deployProject.Enabled = True
        btn_startProject.Enabled = True
        btn_update.Enabled = True
        pf.btn_OK.Enabled = True
    End Sub

    Private Sub DisableButtons()
        btn_deployProject.Enabled = False
        btn_startProject.Enabled = False
        btn_update.Enabled = False
        pf.btn_OK.Enabled = False
    End Sub

    Private Sub btn_startProject_Click(sender As Object, e As EventArgs) Handles btn_startProject.Click
        If File.Exists(".\MASTER_StartUnity.bat") Then
            Dim pfd As New ProjectForm
            DisableButtons()
            pf = pfd
            pf.txt_projectForm.Text = ""
            pf.Text = "Start Project"
            pf.Visible = True
            pf.ProgressBar1.Maximum = (ListBox1.Items.Count - 1) * 1 * 10
            opencmd_start()
            Dim CMDThread2 As New Threading.Thread(AddressOf CMDConfig)
            CMDThread = CMDThread2
            'start cmd thread
            CMDThread.Start()
        Else
            MsgBox("MASTER_StartUnity.bat not found", MsgBoxStyle.Critical, "Not Found")
        End If
    End Sub

    Private Sub checkStartUnity()
        Dim fileBAT As File
        Dim path As String = ".\SLAVE_StartUnity.bat"
        Dim content As String = "echo Startet Unityprojekt auf dem Slave-Rechner %computername% " + vbNewLine + "start " + pathToSlave + projectname + ".exe"
        fileBAT.WriteAllText(path, content)
    End Sub

    Private Sub btn_deployProject_Click(sender As Object, e As EventArgs) Handles btn_deployProject.Click
        If File.Exists(".\ProjektVerteilen.bat") Then
            Dim pfd As New ProjectForm
            DisableButtons()
            checkStartUnity()
            pf = pfd
            pf.txt_projectForm.Text = ""
            pf.Text = "Deploy Project"
            pf.Visible = True
            pf.ProgressBar1.Maximum = (ListBox1.Items.Count - 1) * 3 * 10
            opencmd_deployupdate(False)
            Dim CMDThread2 As New Threading.Thread(AddressOf CMDConfig)
            CMDThread = CMDThread2
            'start cmd thread
            CMDThread.Start()
        Else
            MsgBox("ProjektVerteilen.bat not found", MsgBoxStyle.Critical, "Not Found")
        End If
    End Sub

    Private Sub ÜberDasToolToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ÜberDasToolToolStripMenuItem.Click
        MsgBox("ConfigTool Version 0.0.1" + vbNewLine + "GitHub: http://www.github.com/thechosi/CAVE/", MsgBoxStyle.Information, "ConfigTool")
    End Sub

    Private Sub btn_update_Click(sender As Object, e As EventArgs) Handles btn_update.Click
        If File.Exists(".\ProjektVerteilen.bat") Then
            Dim pfd As New ProjectForm
            DisableButtons()
            checkStartUnity()
            pf = pfd
            pf.txt_projectForm.Text = ""
            pf.Text = "Update Project"
            pf.Visible = True
            pf.ProgressBar1.Maximum = (ListBox1.Items.Count - 1) * 2 * 10
            opencmd_deployupdate(True)
            Dim CMDThread2 As New Threading.Thread(AddressOf CMDConfig)
            CMDThread = CMDThread2
            'start cmd thread
            CMDThread.Start()
        Else
            MsgBox("ProjektVerteilen.bat not found", MsgBoxStyle.Critical, "Not Found")
        End If
    End Sub

    Private Sub opencmd_deploy(v As String)
        Throw New NotImplementedException()
    End Sub

    Private Sub txt_scale_TextChanged(sender As Object, e As EventArgs) Handles txt_scale.TextChanged
        If list_splane.SelectedItem.Equals("Scale") Then
            act_computer.screenplane.scale.x = txt_scale.Text
            act_computer.screenplane.scale.y = txt_scale.Text
            act_computer.screenplane.scale.z = txt_scale.Text
        End If
    End Sub

    Private Sub SlaveConfigToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SlaveConfigToolStripMenuItem.Click

        Dim newPath As String = ""
        newPath = InputBox("Please enter the path to the folder where the .exe-file is located:", "Slave Path", pathToSlave)

        If newPath IsNot "" Then
            pathToSlave = newPath
        End If

    End Sub

End Class