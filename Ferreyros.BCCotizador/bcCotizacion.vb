Imports System.Data.SqlClient
Imports Ferreyros.DACotizador
Imports Ferreyros.BECotizador

Public Class bcCotizacion

    Private pr_strError As String
    Private _daCotizacion As daCotizacion = Nothing

    Public ReadOnly Property ErrorDes() As String
        Get
            Return pr_strError
        End Get
    End Property

    Public Function BuscarId(ByVal strConexion As String, ByRef obeCotizacion As beCotizacion) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaCotizacion As New daCotizacion
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaCotizacion.BuscarId(CnnSql, obeCotizacion) Then
                    Throw New Exception(odaCotizacion.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            pr_strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function

    Public Function ReportePrimeIdCotizacion(ByVal strConexion As String, ByVal eCotizacion As beCotizacion, _
                                      ByRef dtsConsulta As DataSet) As Boolean
        Dim blnResultado As Boolean
        Try
            Dim odaCotizacion As New daCotizacion
            Using cnnSql As New SqlConnection(strConexion)
                cnnSql.Open()
                If Not odaCotizacion.ReportePrimeIdCotizacion(cnnSql, eCotizacion, dtsConsulta) Then
                    Throw New Exception(odaCotizacion.ErrorDes)
                End If
            End Using
            blnResultado = True
        Catch ex As Exception
            pr_strError = ex.Message.ToString
        End Try
        Return blnResultado
    End Function

    Public Function Busqueda(ByVal strConexion As String, ByVal eCotizacion As beCotizacion, _
                                      ByRef dtsConsulta As DataTable) As Boolean
        Dim blnResultado As Boolean
        Try
            Dim odaCotizacion As New daCotizacion
            Using cnnSql As New SqlConnection(strConexion)
                cnnSql.Open()
                If Not odaCotizacion.Busqueda(cnnSql, eCotizacion, dtsConsulta) Then
                    Throw New Exception(odaCotizacion.ErrorDes)
                End If
            End Using
            blnResultado = True
        Catch ex As Exception
            pr_strError = ex.Message.ToString
        End Try
        Return blnResultado
    End Function

    Public Function BusquedaEnAprobacion(ByVal strConexion As String, ByVal eCotizacion As beCotizacion, _
                                      ByRef dtsConsulta As DataTable) As Boolean
        Dim blnResultado As Boolean
        Try
            Dim odaCotizacion As New daCotizacion
            Using cnnSql As New SqlConnection(strConexion)
                cnnSql.Open()
                If Not odaCotizacion.BusquedaEnAprobacion(cnnSql, eCotizacion, dtsConsulta) Then
                    Throw New Exception(odaCotizacion.ErrorDes)
                End If
            End Using
            blnResultado = True
        Catch ex As Exception
            pr_strError = ex.Message.ToString
        End Try
        Return blnResultado
    End Function




    Public Sub ActualizarCotizacionEnvioSAP(ByVal eConexion As String, ByVal eCotizacion As beCotizacion, ByRef eValidacion As beValidacion)

        If String.IsNullOrEmpty(eConexion) OrElse eCotizacion Is Nothing Then
            Return
        End If

        Try
            _daCotizacion = New daCotizacion
            _daCotizacion.ActualizarCotizacionEnvioSAP(eConexion, eCotizacion, eValidacion)
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub

    Public Sub ActualizarSolComb(ByVal eConexion As String, ByVal l_SolComb As List(Of beSolucionCombinada), ByVal idCotizacion As String, ByVal idProducto As String, ByVal idPosicion As String, ByVal idTarifa As String, ByRef eValidacion As beValidacion)

        If String.IsNullOrEmpty(eConexion) OrElse idCotizacion = "" Then
            Return
        End If

        Try
            _daCotizacion = New daCotizacion
            _daCotizacion.ActualizarSolComb(eConexion, l_SolComb, idCotizacion, idProducto, idPosicion, idTarifa, eValidacion)
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub

    Public Sub CotizacionConsultar(ByVal oConexion As String, ByVal Cotizacion As beCotizacion, ByRef oValidacion As beValidacion, ByRef oDetalle As List(Of beCotizacion))

        If String.IsNullOrEmpty(oConexion) OrElse Cotizacion Is Nothing Then
            Return
        End If

        Try
            _daCotizacion = New daCotizacion
            _daCotizacion.CotizacionConsultar(oConexion, Cotizacion, oValidacion, oDetalle)
        Catch ex As Exception
            oValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub

    Public Sub CotizacionListar(ByVal oConexion As String, ByRef Cotizacion As beCotizacion, ByRef oValidacion As beValidacion)

        Try
            _daCotizacion = New daCotizacion
            _daCotizacion.CotizacionListar(oConexion, Cotizacion, oValidacion)
        Catch ex As Exception
            oValidacion.mensaje = ex.Message.ToString
        End Try


    End Sub
    Public Function DatosDocumento(ByVal strConexion As String, ByVal obeCotizacion As beCotizacion, ByRef dtsCotizacion As DataSet) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaCotizacion As New daCotizacion
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaCotizacion.DatosDocumento(CnnSql, obeCotizacion, dtsCotizacion) Then
                    Throw New Exception(odaCotizacion.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            pr_strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function
End Class
