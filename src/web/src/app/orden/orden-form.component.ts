// src/app/orden/orden-form.component.ts
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MecanicoService } from '../services/mecanico.service';
import { OrdenService } from '../services/orden.service';

@Component({
  selector: 'app-orden-form',
  templateUrl: './orden-form.component.html',
  standalone: true,
  styleUrls: ['./orden-form.component.css'],
  imports: [CommonModule, ReactiveFormsModule]
})
export class OrdenFormComponent implements OnInit {
  form!: FormGroup;
  mecanicoRecomendado: any = null;

  @Output() ordenCreada = new EventEmitter<void>(); // Para notificar al listado

  categorias = [
    { value: 1, text: 'Mecánica general' },
    { value: 2, text: 'Electricidad/Electrónica' },
    { value: 3, text: 'Estética/Carrocería' }
  ];

  constructor(
    private fb: FormBuilder,
    private mecSvc: MecanicoService,
    private ordSvc: OrdenService
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      vehiculoID: [null, Validators.required],
      tipoReparacion: [null, Validators.required]
    });

    this.form.get('tipoReparacion')?.valueChanges.subscribe((cat: number) => {
      if (cat) {
        this.mecanicoRecomendado = null;
        this.mecSvc.getByCategoria(cat).subscribe({
          next: data => this.mecanicoRecomendado = data[0] ?? null,
          error: err => console.error('Error cargando mecánicos', err)
        });
      }
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.ordSvc.crearOrden(this.form.value).subscribe({
      next: res => {
        alert('Orden creada exitosamente');
        this.form.reset();
        this.mecanicoRecomendado = null;
        this.ordenCreada.emit(); // Notifica al listado que se actualice
      },
      error: err => {
        console.error('Error creando orden', err);
        alert('Hubo un error al crear la orden');
      }
    });
  }
}
