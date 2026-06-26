Imports System
Imports System.Web.UI
Public Class cancel
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim transactionId As String = Request.QueryString("transaction_id") ' Example query parameter
        If transactionId IsNot Nothing Then
            ' Log the transaction ID as a canceled payment
            Console.WriteLine("Payment cancelled. Transaction ID: " & transactionId)
        End If
    End Sub

End Class