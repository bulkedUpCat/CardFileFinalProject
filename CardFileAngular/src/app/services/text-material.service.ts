import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http'
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CreateTextMaterial } from '../models/CreateTextMaterial';
import { MaterialCategory } from '../models/MaterialCategory';
import { TextMaterial } from '../models/TextMaterial';
import { TextMaterialParameters } from '../models/parameters/TextMaterialParameters';
import { UpdateTextMaterial } from '../models/UpdateTextMaterial';
import { EmailParameters } from '../models/parameters/EmailParameters';

@Injectable({
  providedIn: 'root'
})
export class TextMaterialService {
  public showApproved = new BehaviorSubject<string>(null);

  constructor(private http: HttpClient) { }

  getTextMaterials(textParams: TextMaterialParameters) : Observable<any>{
    var parameters = {};
    if (textParams.userId) parameters['userId'] = textParams.userId;
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
    console.log(textParams);
    return this.http.get(environment.apiUrl + '/textMaterials', {
      params: parameters
    });
  }

  getTextMaterialsByUserId(id: string) : Observable<TextMaterial[]>{
    return this.http.get<TextMaterial[]>(`${environment.apiUrl}/users/${id}/textMaterials`);
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

  approveTextMaterial(id: number){
    return this.http.post<number>(`${environment.apiUrl}/textMaterials/${id}/approve`,null)
  }

  rejectTextMaterial(id: number){
    return this.http.post<number>(`${environment.apiUrl}/textMaterials/${id}/reject`,null);
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
}
