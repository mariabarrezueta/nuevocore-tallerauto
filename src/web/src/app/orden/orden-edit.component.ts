import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { OrdenService } from '../services/orden.service';

@Component({
  selector: 'app-orden-edit',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './orden-edit.component.html'
})
export class OrdenEditComponent {
  idOrdenSeleccionada: number = 0;
  nuevoEstadoSeleccionado: number = 0;
  mensajeConfirmacion: string = '';

  estados: { nombre: string, valor: number }[] = [
    { nombre: 'Pendiente', valor: 0 },
    { nombre: 'En Proceso', valor: 1 },
    { nombre: 'Finalizada', valor: 2 },
    { nombre: 'Entregada', valor: 3 }
  ];

  constructor(private ordenService: OrdenService) {}

  actualizarEstado() {
    const estadoNumerico = Number(this.nuevoEstadoSeleccionado); // 👈 asegura que es un número
    const dto = { estado: estadoNumerico };
  
    this.ordenService.actualizarEstado(this.idOrdenSeleccionada, dto).subscribe({
      next: () => {
        this.mensajeConfirmacion = `✅ Estado actualizado correctamente para la orden #${this.idOrdenSeleccionada}`;
      },
      error: (err) => {
        console.error('❌ Error al actualizar estado:', err);
        this.mensajeConfirmacion = '❌ Error al actualizar el estado';
      }
    });
  }
  
  
  
}











