import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { MaterialCategory } from '../models/MaterialCategory';

@Injectable({
  providedIn: 'root'
})
export class MaterialCategoryService {

  constructor(private http: HttpClient) { }

  getMaterialCategories(): Observable<MaterialCategory[]>{
    return this.http.get<MaterialCategory[]>(`${environment.apiUrl}/textMaterials/categories`);
  }

  getMaterialCategoryById(id: number): Observable<MaterialCategory>{
    return this.http.get<MaterialCategory>(`${environment.apiUrl}/textMaterials/categories/` + id)
  }
}
