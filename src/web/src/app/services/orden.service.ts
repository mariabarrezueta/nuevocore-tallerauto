import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable, EventEmitter } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class OrdenService {
  private api = 'http://localhost:5000/api/Ordenes';
  recargar = new EventEmitter<void>();
  
  constructor(private http: HttpClient) {}

  crearOrden(data: any) {
    return this.http.post(this.api, data);
  }
    getOrdenes() {
      return this.http.get<any[]>(this.api);
    }
    
    actualizarEstado(id: number, dto: { estado: number }): Observable<any> {
      return this.http.put(`${this.api}/${id}/estado`, dto);
    }
    
    
    
    
    
    
    
    

    
    
    
    
    
}

