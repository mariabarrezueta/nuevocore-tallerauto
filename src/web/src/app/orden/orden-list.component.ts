// src/app/orden/orden-list.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdenService } from '../services/orden.service';

@Component({
  selector: 'app-orden-list',
  templateUrl: './orden-list.component.html',
  standalone: true,
  styleUrls: ['./orden-list.component.css'],
  imports: [CommonModule]
})
export class OrdenListComponent implements OnInit {
  ordenes: any[] = [];

  constructor(private ordSvc: OrdenService) {}

  ngOnInit(): void {
    this.cargarOrdenes();
  }

  cargarOrdenes(): void {
    this.ordSvc.getOrdenes().subscribe({
      next: data => {
        console.log('Órdenes recibidas:', data);  // 👈 Esto imprime los datos en consola
        this.ordenes = data;
      },
      error: err => console.error('Error cargando órdenes', err)
    });
  }
  
  
}
