﻿Imports System.IO

Public Class MainForm
    Dim computers As New List(Of Computer)
    Dim act_computer As Computer
    Dim filename As String
    Dim foldername As String

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
            filename = FolderBrowserDialog1.SelectedPath + "\Assets\StreamingAssets\node-config.xml"
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
                txt_projectname.Text = FolderBrowserDialog1.SelectedPath.Substring(FolderBrowserDialog1.SelectedPath.LastIndexOf("\") + 1)
            Else
                MsgBox("Configuration file not found. Please choose correct directory")
            End If
        End If
    End Sub

    Private Sub list_rela_SelectedIndexChanged(sender As Object, e As EventArgs) Handles list_rela.SelectedIndexChanged

        Dim relation As RelationToOrigin = DirectCast(act_computer, Master).relationToOrigin

        If list_rela.SelectedItem.Equals("position") Then
            txt_relat_x.Text = relation.position.x
            txt_relat_y.Text = relation.position.y
            txt_relat_z.Text = relation.position.z
        ElseIf list_rela.SelectedItem.Equals("rotation") Then
            txt_relat_x.Text = relation.rotation.x
            txt_relat_y.Text = relation.rotation.y
            txt_relat_z.Text = relation.rotation.z
        End If


    End Sub

    Private Sub list_splane_SelectedIndexChanged(sender As Object, e As EventArgs) Handles list_splane.SelectedIndexChanged


        Dim screenplane As ScreenPlane = act_computer.screenplane

        If list_splane.SelectedItem.Equals("pa") Then
            txt_splane_x.Text = screenplane.pa.x
            txt_splane_y.Text = screenplane.pa.y
            txt_splane_z.Text = screenplane.pa.z
        ElseIf list_splane.SelectedItem.Equals("pb") Then
            txt_splane_x.Text = screenplane.pb.x
            txt_splane_y.Text = screenplane.pb.y
            txt_splane_z.Text = screenplane.pb.z
        ElseIf list_splane.SelectedItem.Equals("pc") Then
            txt_splane_x.Text = screenplane.pc.x
            txt_splane_y.Text = screenplane.pc.y
            txt_splane_z.Text = screenplane.pc.z
        ElseIf list_splane.SelectedItem.Equals("pe") Then
            txt_splane_x.Text = screenplane.pe.x
            txt_splane_y.Text = screenplane.pe.y
            txt_splane_z.Text = screenplane.pe.z
        End If
    End Sub

    Private Sub list_cam_SelectedIndexChanged(sender As Object, e As EventArgs) Handles list_cam.SelectedIndexChanged

        Dim camera As Camera = DirectCast(act_computer, Slave).camera

        If list_cam.SelectedItem.Equals("rotation") Then
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

        If list_rela.SelectedItem.Equals("position") Then
            DirectCast(act_computer, Master).relationToOrigin.position.x = txt_relat_x.Text
        ElseIf list_rela.SelectedItem.Equals("rotation") Then
            DirectCast(act_computer, Master).relationToOrigin.rotation.x = txt_relat_x.Text
        End If

    End Sub

    Private Sub txt_cam_x_TextChanged(sender As Object, e As EventArgs) Handles txt_cam_x.TextChanged
        DirectCast(act_computer, Slave).camera.rotation.x = txt_cam_x.Text
    End Sub

    Private Sub txt_splane_x_TextChanged(sender As Object, e As EventArgs) Handles txt_splane_x.TextChanged
        If list_splane.SelectedItem.Equals("pa") Then
            act_computer.screenplane.pa.x = txt_splane_x.Text
        ElseIf list_splane.SelectedItem.Equals("pb") Then
            act_computer.screenplane.pb.x = txt_splane_x.Text
        ElseIf list_splane.SelectedItem.Equals("pc") Then
            act_computer.screenplane.pc.x = txt_splane_x.Text
        ElseIf list_splane.SelectedItem.Equals("pe") Then
            act_computer.screenplane.pe.x = txt_splane_x.Text
        End If
    End Sub

    Private Sub txt_relat_y_TextChanged(sender As Object, e As EventArgs) Handles txt_relat_y.TextChanged
        If list_rela.SelectedItem.Equals("position") Then
            DirectCast(act_computer, Master).relationToOrigin.position.y = txt_relat_y.Text
        ElseIf list_rela.SelectedItem.Equals("rotation") Then
            DirectCast(act_computer, Master).relationToOrigin.rotation.y = txt_relat_y.Text
        End If
    End Sub

    Private Sub txt_relat_z_TextChanged(sender As Object, e As EventArgs) Handles txt_relat_z.TextChanged
        If list_rela.SelectedItem.Equals("position") Then
            DirectCast(act_computer, Master).relationToOrigin.position.z = txt_relat_z.Text
        ElseIf list_rela.SelectedItem.Equals("rotation") Then
            DirectCast(act_computer, Master).relationToOrigin.rotation.z = txt_relat_z.Text
        End If
    End Sub

    Private Sub txt_cam_y_TextChanged(sender As Object, e As EventArgs) Handles txt_cam_y.TextChanged
        DirectCast(act_computer, Slave).camera.rotation.y = txt_cam_y.Text
    End Sub

    Private Sub txt_cam_z_TextChanged(sender As Object, e As EventArgs) Handles txt_cam_z.TextChanged
        DirectCast(act_computer, Slave).camera.rotation.z = txt_cam_z.Text
    End Sub

    Private Sub txt_splane_y_TextChanged(sender As Object, e As EventArgs) Handles txt_splane_y.TextChanged
        If list_splane.SelectedItem.Equals("pa") Then
            act_computer.screenplane.pa.y = txt_splane_y.Text
        ElseIf list_splane.SelectedItem.Equals("pb") Then
            act_computer.screenplane.pb.y = txt_splane_y.Text
        ElseIf list_splane.SelectedItem.Equals("pc") Then
            act_computer.screenplane.pc.y = txt_splane_y.Text
        ElseIf list_splane.SelectedItem.Equals("pe") Then
            act_computer.screenplane.pe.y = txt_splane_y.Text
        End If
    End Sub

    Private Sub txt_splane_z_TextChanged(sender As Object, e As EventArgs) Handles txt_splane_z.TextChanged
        If list_splane.SelectedItem.Equals("pa") Then
            act_computer.screenplane.pa.z = txt_splane_z.Text
        ElseIf list_splane.SelectedItem.Equals("pb") Then
            act_computer.screenplane.pb.z = txt_splane_z.Text
        ElseIf list_splane.SelectedItem.Equals("pc") Then
            act_computer.screenplane.pc.z = txt_splane_z.Text
        ElseIf list_splane.SelectedItem.Equals("pe") Then
            act_computer.screenplane.pe.z = txt_splane_z.Text
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

    Private Sub btn_startProject_Click(sender As Object, e As EventArgs) Handles btn_startProject.Click
        Dim pf As New ProjectForm
        pf.Text = "Start Project"
        pf.Visible = True
        pf.ProgressBar1.Value = 20
        Using P As Process = New Process
            P.StartInfo.CreateNoWindow = True
            P.StartInfo.UseShellExecute = False
            P.StartInfo.RedirectStandardInput = True
            P.StartInfo.RedirectStandardOutput = True
            P.StartInfo.RedirectStandardError = True
            P.StartInfo.FileName = foldername + "\start.bat"
            P.Start()
            pf.ProgressBar1.Value = 50
            Dim sOutput As String
            Using oStreamReader As System.IO.StreamReader = P.StandardOutput
                sOutput = oStreamReader.ReadToEnd()
            End Using
            pf.ProgressBar1.Value = 90
            pf.txt_projectForm.Text = sOutput
            'Cursor ans Ende setzen:
            pf.txt_projectForm.SelectionStart = pf.txt_projectForm.Text.Length
            'Bis zum Cursor scrollen:
            pf.txt_projectForm.ScrollToCaret()
            P.Close()
            pf.ProgressBar1.Value = 100

        End Using

    End Sub

    Private Sub btn_deployProject_Click(sender As Object, e As EventArgs) Handles btn_deployProject.Click
        Dim pf As New ProjectForm
        pf.Text = "Deploy Project"
        pf.Visible = True
        pf.ProgressBar1.Value = 20
        Using P As Process = New Process
            P.StartInfo.CreateNoWindow = True
            P.StartInfo.UseShellExecute = False
            P.StartInfo.RedirectStandardInput = True
            P.StartInfo.RedirectStandardOutput = True
            P.StartInfo.RedirectStandardError = True
            P.StartInfo.FileName = foldername + "\deploy.bat"
            P.Start()
            pf.ProgressBar1.Value = 50
            Dim sOutput As String
            Using oStreamReader As System.IO.StreamReader = P.StandardOutput
                sOutput = oStreamReader.ReadToEnd()
            End Using
            pf.ProgressBar1.Value = 80
            pf.txt_projectForm.Text = sOutput
            'Cursor ans Ende setzen:
            pf.txt_projectForm.SelectionStart = pf.txt_projectForm.Text.Length
            'Bis zum Cursor scrollen:
            pf.txt_projectForm.ScrollToCaret()
            P.Close()
            pf.ProgressBar1.Value = 100

        End Using
    End Sub

    Private Sub ÜberDasToolToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ÜberDasToolToolStripMenuItem.Click
        MsgBox("ConfigTool Version 0.0.1", MsgBoxStyle.Information, "ConfigTool")
    End Sub
End Class