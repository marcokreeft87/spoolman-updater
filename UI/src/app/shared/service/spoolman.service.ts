import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { UpdateTrayInput } from '../models/updatetray';
import { Spool } from '../models/spool';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class SpoolsService {
  private baseUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  /**
   * GET /Spools
   */
  getSpools(): Observable<Spool[]> {
    return this.http.get<any>(`${this.baseUrl}spools`).pipe(map(response => response.spools));
  }

  /**
   * POST /Spools/tray
   */
  updateTray(data: UpdateTrayInput): Observable<Spool> {
    return this.http.post<any>(`${this.baseUrl}spools/tray`, data).pipe(map(response => response.spool));
  }

  getByBarcode(barcode: string): Observable<Spool[]> {
    const params = new HttpParams()
      .set('barcode', barcode); // Example barcode, replace with actual value

    return this.http.get<any>(`${this.baseUrl}spools/barcode`, { params }).pipe(map(response => response.spools));
  }
}
