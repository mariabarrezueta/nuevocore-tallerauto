import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class MecanicoService {
  private api = 'http://localhost:60352/api';   // ajusta URL production

  constructor(private http: HttpClient) {}

  getByCategoria(cat: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.api}/mecanicos?categoriaId=${cat}`);
  
  }

  getMecanicosActivos(): Observable<any[]> {
    return this.http.get<any[]>(`${this.api}/mecanicos/activos`);
  }

}
