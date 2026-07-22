import { useState } from 'react'
import { api } from '../api/client'
import { AyudaFormulario } from '../components/AyudaFormulario'

const AYUDA_CLIENTES = [
  'El DNI debe tener exactamente 8 dígitos y es único por cliente.',
  "Usa 'Buscar por DNI' antes de registrar, para evitar duplicar un cliente ya existente.",
  'Teléfono y correo son opcionales.',
]

export function ClientesPage() {
  const [dni, setDni] = useState('')
  const [cliente, setCliente] = useState(null)
  const [mensaje, setMensaje] = useState(null)
  const [form, setForm] = useState({ dni: '', nombres: '', apellidos: '', telefono: '', correo: '' })

  async function buscar() {
    setMensaje(null)
    setCliente(null)
    try {
      const encontrado = await api.get(`/api/clientes/dni/${dni}`)
      setCliente(encontrado)
    } catch (err) {
      setMensaje('No se encontro un cliente con ese DNI.')
    }
  }

  function actualizarCampo(campo, valor) {
    setForm((prev) => ({ ...prev, [campo]: valor }))
  }

  async function registrar(e) {
    e.preventDefault()
    setMensaje(null)
    try {
      const nuevo = await api.post('/api/clientes', {
        ...form,
        fechaNacimiento: null,
        direccion: null,
      })
      setMensaje('Cliente registrado.')
      setCliente(nuevo)
    } catch (err) {
      setMensaje(err.message)
    }
  }

  return (
    <section>
      <h1>
        Clientes
        <AyudaFormulario titulo="Cómo registrar un cliente" pasos={AYUDA_CLIENTES} />
      </h1>

      <div className="tarjeta">
        <h3>Buscar por DNI</h3>
        <label>
          DNI
          <input value={dni} onChange={(e) => setDni(e.target.value)} maxLength={8} />
        </label>
        <button onClick={buscar}>Buscar</button>

        {cliente && (
          <div className="resultado">
            <p><strong>{cliente.nombres} {cliente.apellidos}</strong></p>
            <p>DNI: {cliente.dni} — Estado: {cliente.estado}</p>
            <p>Telefono: {cliente.telefono || '-'} — Correo: {cliente.correo || '-'}</p>
          </div>
        )}
      </div>

      <form className="tarjeta" onSubmit={registrar}>
        <h3>Registrar cliente</h3>
        <label>
          DNI
          <input value={form.dni} onChange={(e) => actualizarCampo('dni', e.target.value)} maxLength={8} required />
        </label>
        <label>
          Nombres
          <input value={form.nombres} onChange={(e) => actualizarCampo('nombres', e.target.value)} required />
        </label>
        <label>
          Apellidos
          <input value={form.apellidos} onChange={(e) => actualizarCampo('apellidos', e.target.value)} required />
        </label>
        <label>
          Telefono
          <input value={form.telefono} onChange={(e) => actualizarCampo('telefono', e.target.value)} />
        </label>
        <label>
          Correo
          <input value={form.correo} onChange={(e) => actualizarCampo('correo', e.target.value)} />
        </label>
        <button type="submit">Registrar</button>
      </form>

      {mensaje && <p className="aviso">{mensaje}</p>}
    </section>
  )
}
