Imports Microsoft.VisualBasic
Imports System
Imports System.Configuration
Imports System.Collections.Generic
Imports System.Web

Public Class AppGlobals
    Public Shared Property ConnectionString As String
    Public Shared Property CachedLists As Dictionary(Of String, List(Of Pair))

    Shared Sub New()
        CachedLists = New Dictionary(Of String, List(Of Pair))()

    End Sub
End Class
