
Public Class Computer
    Public ip As String = ""
    Public screenplane As New ScreenPlane

    'DeployInfos
    Public name As String = ""
    Public username As String = ""


    Public Overridable Function ToString()
        Return ip
    End Function

End Class

Public Class Master : Inherits Computer
    Public relationToOrigin As New RelationToOrigin
    Public port As String = ""

    Public Overrides Function ToString()
        Return MyBase.ToString & " (Master)"
    End Function

End Class

Public Class Slave : Inherits Computer
    Public camera As New Camera

    Public Overrides Function ToString()
        Return MyBase.ToString & " (Slave)"
    End Function
End Class

Public Class RelationToOrigin
    Public position As New Vektor
    Public rotation As New Vektor
End Class

Public Class Vektor
    Public x As String
    Public y As String
    Public z As String
End Class

Public Class ScreenPlane
    Public pa As New Vektor
    Public pb As New Vektor
    Public pc As New Vektor
    Public pe As New Vektor
End Class

Public Class Camera
    Public eye As String = ""
    Public rotation As New Vektor
End Class