import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MecanicoService } from '../services/mecanico.service';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-mecanicos-activos',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  templateUrl: './mecanicos-activos.component.html',
  styleUrls: ['./mecanicos-activos.component.scss']
})
export class MecanicosActivosComponent implements OnInit {
  mecanicos: any[] = [];

  constructor(private mecanicoService: MecanicoService) {}

  ngOnInit(): void {
    this.cargarMecanicos();
  }

  cargarMecanicos(): void {
    this.mecanicoService.getMecanicosActivos().subscribe({
      next: data => {
        this.mecanicos = data.sort((a: any, b: any) => b.ordenesActivas - a.ordenesActivas);
      },
      error: err => console.error('Error cargando mecánicos', err)
    });
  }

  ordenarPorOrdenes(): void {
    this.mecanicos.sort((a, b) => b.ordenesActivas - a.ordenesActivas);
  }
}
