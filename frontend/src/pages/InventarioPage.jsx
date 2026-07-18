import { useEffect, useState } from 'react'
import { api } from '../api/client'

export function InventarioPage() {
  const [locales, setLocales] = useState([])
  const [productos, setProductos] = useState([])
  const [localId, setLocalId] = useState('')
  const [inventario, setInventario] = useState([])
  const [mensaje, setMensaje] = useState(null)

  useEffect(() => {
    api.get('/api/locales').then(setLocales)
    api.get('/api/productos').then(setProductos)
  }, [])

  useEffect(() => {
    if (!localId) {
      setInventario([])
      return
    }
    setMensaje(null)
    api.get('/api/inventarios', { localId }).then(setInventario).catch((e) => setMensaje(e.message))
  }, [localId])

  function nombreProducto(productoId) {
    return productos.find((p) => p.id === productoId)?.nombreComercial ?? productoId
  }

  return (
    <section>
      <h1>Inventario</h1>

      <div className="tarjeta">
        <label>
          Local
          <select value={localId} onChange={(e) => setLocalId(e.target.value)}>
            <option value="">--</option>
            {locales.map((l) => <option key={l.id} value={l.id}>{l.nombre}</option>)}
          </select>
        </label>

        {mensaje && <p className="aviso">{mensaje}</p>}

        <table>
          <thead>
            <tr><th>Producto</th><th>Cantidad actual</th><th>Actualizado</th></tr>
          </thead>
          <tbody>
            {inventario.map((i) => (
              <tr key={i.productoId}>
                <td>{nombreProducto(i.productoId)}</td>
                <td>{i.cantidadActual}</td>
                <td>{new Date(i.actualizadoEn).toLocaleString()}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </section>
  )
}
