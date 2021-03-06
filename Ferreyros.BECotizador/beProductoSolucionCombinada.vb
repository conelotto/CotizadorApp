﻿<Serializable()>
Public Class beProductoSolucionCombinada
    Public Property IdProducto() As String = String.Empty
    Public Property FechaEstimCierre() As String = String.Empty
    Public Property PlazoEntregaEstim() As String = String.Empty
    Public Property CodigoFormaPago() As String = String.Empty
    Public Property FormaPago() As String = String.Empty
    Public Property FlatIncluyeRecompra() As String = String.Empty
    Public Property FlatIncluyeCLC() As String = String.Empty
    Public Property PromHorasMensualUso() As String = String.Empty
    Public Property AnioFabricacion() As String = String.Empty
    Public Property Garantia() As String = String.Empty
    Public Property Condicion() As String = String.Empty
    Public Property PlazoEntrega() As String = String.Empty
    Public Property Orden() As String = String.Empty
    Public Property Serie() As String = String.Empty
    Public Property Horas() As String = String.Empty
    Public Property CodClasificacion() As String = String.Empty

    Public Property UsuarioCreacion() As String = String.Empty
    Public Property ListabeDetalleRecompra() As New List(Of beDetalleRecompra)
    Public Property ListabeProductoAdicional() As New List(Of beProductoAdicional)
    Public Property ListabeProductoAccesorio() As New List(Of beProductoAccesorio)
End Class
