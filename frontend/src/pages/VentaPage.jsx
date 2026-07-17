import { useEffect, useState } from 'react'
import { api } from '../api/client'
import { useAuth } from '../auth/AuthContext'

export function VentaPage() {
  const { session } = useAuth()
  const [cajas, setCajas] = useState([])
  const [productos, setProductos] = useState([])
  const [formasPago, setFormasPago] = useState([])
  const [convenios, setConvenios] = useState([])
  const [cajaId, setCajaId] = useState('')
  const [clienteDni, setClienteDni] = useState('')
  const [venta, setVenta] = useState(null)
  const [mensaje, setMensaje] = useState(null)

  const [productoId, setProductoId] = useState('')
  const [cantidad, setCantidad] = useState('1')
  const [recetaId, setRecetaId] = useState('')

  const [formaPagoId, setFormaPagoId] = useState('')
  const [montoPago, setMontoPago] = useState('')
  const [convenioId, setConvenioId] = useState('')
  const [serieComprobante, setSerieComprobante] = useState('B001')
  const [tipoComprobante, setTipoComprobante] = useState('Boleta')

  useEffect(() => {
    api.get('/api/cajas').then(setCajas)
    api.get('/api/productos').then(setProductos)
    api.get('/api/formas-pago').then(setFormasPago)
  }, [])

  function mostrarError(err) {
    setMensaje(err.message)
  }

  async function iniciarVenta() {
    setMensaje(null)
    try {
      const sesion = await api.get(`/api/cajas/${cajaId}/sesion-activa`)
      const nueva = await api.post('/api/ventas', {
        cajaId,
        sesionCajaId: sesion.id,
        usuarioId: session.usuarioId,
        clienteDni: clienteDni || null,
      })
      setVenta(nueva)
    } catch (err) {
      mostrarError(err)
    }
  }

  async function agregarProducto() {
    setMensaje(null)
    try {
      const actualizada = await api.post(`/api/ventas/${venta.id}/detalles`, {
        productoId,
        cantidad: Number(cantidad),
        recetaId: recetaId || null,
      })
      setVenta(actualizada)
      setProductoId('')
      setCantidad('1')
      setRecetaId('')
    } catch (err) {
      mostrarError(err)
    }
  }

  async function verPromociones(detalleId) {
    setMensaje(null)
    try {
      const disponibles = await api.get(`/api/ventas/${venta.id}/promociones-disponibles`, { detalleVentaId: detalleId })
      if (disponibles.length === 0) {
        setMensaje('No hay promociones aplicables para esta linea.')
        return
      }
      const elegida = disponibles[0]
      const actualizada = await api.patch(`/api/ventas/${venta.id}/detalles/${detalleId}/promocion`, { promocionId: elegida.id })
      setVenta(actualizada)
      setMensaje(`Promocion aplicada: ${elegida.nombre}`)
    } catch (err) {
      mostrarError(err)
    }
  }

  async function identificarCliente() {
    setMensaje(null)
    try {
      const actualizada = await api.post(`/api/ventas/${venta.id}/cliente`, { dni: clienteDni })
      setVenta(actualizada)
    } catch (err) {
      mostrarError(err)
    }
  }

  async function aplicarConvenio() {
    setMensaje(null)
    try {
      const copago = await api.post(`/api/ventas/${venta.id}/convenio`, { convenioId })
      setMensaje(`Cubierto: S/ ${copago.montoCubierto} — Copago del cliente: S/ ${copago.copago}`)
      const actualizada = await api.get(`/api/ventas/${venta.id}`)
      setVenta(actualizada)
    } catch (err) {
      mostrarError(err)
    }
  }

  async function registrarPago() {
    setMensaje(null)
    try {
      const actualizada = await api.post(`/api/ventas/${venta.id}/pagos`, {
        formaPagoId,
        monto: Number(montoPago),
        codigoAutorizacion: null,
      })
      setVenta(actualizada)
      setMontoPago('')
    } catch (err) {
      mostrarError(err)
    }
  }

  async function confirmarVenta() {
    setMensaje(null)
    try {
      const confirmada = await api.post(`/api/ventas/${venta.id}/confirmar`, { tipoComprobante, serieComprobante })
      setVenta(confirmada)
      setMensaje(`Venta confirmada. Comprobante ${confirmada.numeroComprobante}`)
    } catch (err) {
      mostrarError(err)
    }
  }

  function nuevaVenta() {
    setVenta(null)
    setClienteDni('')
    setMensaje(null)
  }

  if (!venta) {
    return (
      <section>
        <h1>Punto de venta</h1>
        <div className="tarjeta">
          <label>
            Caja
            <select value={cajaId} onChange={(e) => setCajaId(e.target.value)}>
              <option value="">-- Selecciona una caja --</option>
              {cajas.map((c) => <option key={c.id} value={c.id}>{c.nombre}</option>)}
            </select>
          </label>
          <label>
            DNI del cliente (opcional)
            <input value={clienteDni} onChange={(e) => setClienteDni(e.target.value)} maxLength={8} />
          </label>
          <button onClick={iniciarVenta} disabled={!cajaId}>Iniciar venta</button>
          {mensaje && <p className="error">{mensaje}</p>}
        </div>
      </section>
    )
  }

  const ventaConfirmada = venta.estado === 'Confirmada'

  return (
    <section>
      <h1>Venta en curso {venta.numeroComprobante ? `— ${venta.numeroComprobante}` : ''}</h1>
      <p>Estado: <strong>{venta.estado}</strong> — Cliente: {venta.clienteId || 'sin identificar'}</p>
      {mensaje && <p className="aviso">{mensaje}</p>}

      <div className="tarjeta">
        <h3>Lineas de venta</h3>
        <table>
          <thead><tr><th>Producto</th><th>Cant.</th><th>Precio</th><th>Descuento</th><th>Subtotal</th><th></th></tr></thead>
          <tbody>
            {venta.detalles.map((d) => (
              <tr key={d.id}>
                <td>{d.nombreProducto}</td>
                <td>{d.cantidad}</td>
                <td>S/ {d.precioUnitario}</td>
                <td>S/ {d.descuentoMonto}</td>
                <td>S/ {d.subtotal}</td>
                <td>{!ventaConfirmada && <button onClick={() => verPromociones(d.id)}>Ver promociones</button>}</td>
              </tr>
            ))}
          </tbody>
        </table>
        <p>Total: <strong>S/ {venta.total}</strong> — Pagado: S/ {venta.totalPagado}</p>
      </div>

      {!ventaConfirmada && (
        <>
          <div className="tarjeta">
            <h3>Agregar producto</h3>
            <label>
              Producto
              <select value={productoId} onChange={(e) => setProductoId(e.target.value)}>
                <option value="">--</option>
                {productos.map((p) => <option key={p.id} value={p.id}>{p.nombreComercial} (S/ {p.precioVenta})</option>)}
              </select>
            </label>
            <label>
              Cantidad
              <input type="number" min="1" value={cantidad} onChange={(e) => setCantidad(e.target.value)} />
            </label>
            <label>
              Id de receta (si el producto es controlado)
              <input value={recetaId} onChange={(e) => setRecetaId(e.target.value)} />
            </label>
            <button onClick={agregarProducto} disabled={!productoId}>Agregar</button>
          </div>

          <div className="tarjeta">
            <h3>Cliente / Convenio</h3>
            <label>
              DNI
              <input value={clienteDni} onChange={(e) => setClienteDni(e.target.value)} maxLength={8} />
            </label>
            <button onClick={identificarCliente} disabled={!clienteDni}>Identificar cliente</button>
            <label>
              Id de convenio
              <input value={convenioId} onChange={(e) => setConvenioId(e.target.value)} />
            </label>
            <button onClick={aplicarConvenio} disabled={!convenioId}>Aplicar convenio</button>
          </div>

          <div className="tarjeta">
            <h3>Registrar pago</h3>
            <label>
              Forma de pago
              <select value={formaPagoId} onChange={(e) => setFormaPagoId(e.target.value)}>
                <option value="">--</option>
                {formasPago.map((f) => <option key={f.id} value={f.id}>{f.nombre}</option>)}
              </select>
            </label>
            <label>
              Monto
              <input type="number" step="0.01" value={montoPago} onChange={(e) => setMontoPago(e.target.value)} />
            </label>
            <button onClick={registrarPago} disabled={!formaPagoId || !montoPago}>Registrar pago</button>
          </div>

          <div className="tarjeta">
            <h3>Confirmar venta</h3>
            <label>
              Tipo de comprobante
              <select value={tipoComprobante} onChange={(e) => setTipoComprobante(e.target.value)}>
                <option value="Boleta">Boleta</option>
                <option value="Factura">Factura</option>
                <option value="Ticket">Ticket</option>
              </select>
            </label>
            <label>
              Serie
              <input value={serieComprobante} onChange={(e) => setSerieComprobante(e.target.value)} />
            </label>
            <button onClick={confirmarVenta} disabled={venta.detalles.length === 0}>Confirmar venta</button>
          </div>
        </>
      )}

      <button onClick={nuevaVenta}>Iniciar otra venta</button>
    </section>
  )
}
