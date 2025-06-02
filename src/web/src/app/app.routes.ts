import { Routes } from '@angular/router';
import { OrdenFormComponent } from './orden/orden-form.component';
import { OrdenListComponent } from './orden/orden-list.component';
import { OrdenEditComponent } from './orden/orden-edit.component';

export const routes: Routes = [
  { path: '', component: OrdenFormComponent }, // Página principal (crear orden)
  { path: 'ordenes', component: OrdenListComponent }, // Ver listado
  { path: 'editar-ordenes', component: OrdenEditComponent } // Editar estado
];
