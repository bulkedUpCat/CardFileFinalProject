import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http'
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CreateTextMaterial } from '../models/CreateTextMaterial';
import { MaterialCategory } from '../models/MaterialCategory';
import { TextMaterial } from '../models/TextMaterial';
import { TextMaterialParameters } from '../models/parameters/TextMaterialParameters';
import { UpdateTextMaterial } from '../models/UpdateTextMaterial';

@Injectable({
  providedIn: 'root'
})
export class TextMaterialService {
  public showApproved = new BehaviorSubject<string>(null);

  constructor(private http: HttpClient) { }

  getTextMaterials(textParams: TextMaterialParameters) : Observable<any>{
    var parameters = {};

    if (textParams.pageNumber) parameters['pageNumber'] = textParams.pageNumber;
    if (textParams.pageSize) parameters['pageSize'] = textParams.pageSize;
    if (textParams.filterFromDate) parameters['startDate'] = textParams.filterFromDate;
    if (textParams.filterToDate) parameters['endDate'] = textParams.filterToDate;
    if (textParams.searchTitle) parameters['searchTitle'] = textParams.searchTitle;
    if (textParams.searchCategory) parameters['searchCategory'] = textParams.searchCategory;
    if (textParams.searchAuthor) parameters['searchAuthor'] = textParams.searchAuthor;
    if (textParams.orderBy) parameters['orderBy'] = textParams.orderBy;
    if (textParams.approvalStatus){
      parameters['approvalStatus'] = textParams.approvalStatus;
    }

    return this.http.get<TextMaterial[]>(environment.apiUrl + '/textMaterials', {
      responseType: 'json',
      observe: 'response',
      params: parameters
    });
  }

  getSavedTextMaterials(userId: string, textParams: TextMaterialParameters): Observable<any>{
    var parameters = {};

    if (textParams.pageNumber) parameters['pageNumber'] = textParams.pageNumber;
    if (textParams.pageSize) parameters['pageSize'] = textParams.pageSize;
    if (textParams.filterFromDate) parameters['startDate'] = textParams.filterFromDate;
    if (textParams.filterToDate) parameters['endDate'] = textParams.filterToDate;
    if (textParams.searchTitle) parameters['searchTitle'] = textParams.searchTitle;
    if (textParams.searchCategory) parameters['searchCategory'] = textParams.searchCategory;
    if (textParams.searchAuthor) parameters['searchAuthor'] = textParams.searchAuthor;
    if (textParams.orderBy) parameters['orderBy'] = textParams.orderBy;
    if (textParams.approvalStatus){
      parameters['approvalStatus'] = textParams.approvalStatus;
    }

    return this.http.get<TextMaterial[]>(`${environment.apiUrl}/users/${userId}/textMaterials/saved`, {
      responseType: 'json',
      observe: 'response',
      params: parameters
    });
  }

  getTextMaterialsByUserId(id: string, textParams: TextMaterialParameters) : Observable<any>{
    var parameters = {};

    if (textParams.pageNumber) parameters['pageNumber'] = textParams.pageNumber;
    if (textParams.pageSize) parameters['pageSize'] = textParams.pageSize;
    if (textParams.filterFromDate) parameters['startDate'] = textParams.filterFromDate;
    if (textParams.filterToDate) parameters['endDate'] = textParams.filterToDate;
    if (textParams.searchTitle) parameters['searchTitle'] = textParams.searchTitle;
    if (textParams.searchCategory) parameters['searchCategory'] = textParams.searchCategory;
    if (textParams.orderBy) parameters['orderBy'] = textParams.orderBy;
    if (textParams.approvalStatus){
      parameters['approvalStatus'] = textParams.approvalStatus;
    }

    return this.http.get<TextMaterial[]>(`${environment.apiUrl}/users/${id}/textMaterials`, {
      responseType: 'json',
      observe: 'response',
      params: parameters
    });
  }

  getTextMaterialsByCategory(category: MaterialCategory) : Observable<TextMaterial[]>{
    return this.http.get<TextMaterial[]>(`${environment.apiUrl}/textMaterials`,{
      params: {
        id: category.id,
        title: category.title
      }
    });
  }

  getTextMaterialById(id: number) : Observable<TextMaterial>{
    return this.http.get<TextMaterial>(`${environment.apiUrl}/textMaterials/`+ id);
  }

  createTextMaterial(textMaterial: CreateTextMaterial){
    const headers = {
      headers: new HttpHeaders({
        'Content-Type' : 'application/json'
      })
    };

    return this.http.post<TextMaterial>(`${environment.apiUrl}/textMaterials`, textMaterial, headers);
  }

  updateTextMaterial(textMaterial: UpdateTextMaterial){
    return this.http.put<number>(`${environment.apiUrl}/textMaterials`,textMaterial);
  }

  deleteTextMaterial(id: number){
    return this.http.delete(`${environment.apiUrl}/textMaterials/${id}`);
  }

  approveTextMaterial(id: number){
    return this.http.put<number>(`${environment.apiUrl}/textMaterials/${id}/approve`,null)
  }

  rejectTextMaterial(id: number, rejectMessage: string){
    let obj = {
      rejectMessage: rejectMessage
    };
    return this.http.put<number>(`${environment.apiUrl}/textMaterials/${id}/reject`, obj);
  }

  sendTextMaterialAsPdf(textMaterialId: number, emailParams: any){
    var parameters = {};

    if (emailParams.title) parameters['title'] = emailParams.title;
    if (emailParams.category) parameters['category'] = emailParams.category;
    if (emailParams.author) parameters['author'] = emailParams.author;
    if (emailParams.datePublished) parameters['datePublished'] = emailParams.datePublished;

    return this.http.get(`${environment.apiUrl}/textMaterials/${textMaterialId}/print`,{
      params: parameters
    });
  }

  addTextMaterialToSaved(userId: string, textMaterialId: number){
    const headers = {
      headers: new HttpHeaders({
        'Content-Type' : 'application/json'
      })
    };

    return this.http.post(`${environment.apiUrl}/users/${userId}/textMaterials/saved`,textMaterialId,headers);
  }

  removeTextMaterialFromSaved(userId: string, textMaterialId: number){
    return this.http.delete(`${environment.apiUrl}/users/${userId}/textMaterials/saved`, {
      body: textMaterialId
    });
  }
}
