import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { UpdateTrayInput } from '../models/updatetray';
import { Spool } from '../models/spool';

@Injectable({
  providedIn: 'root',
})
export class SpoolsService {
  private baseUrl = '/'; // change if your API is prefixed

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
  updateTray(data: UpdateTrayInput): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}spools/tray`, data);
  }

  getByBarcode(barcode: string): Observable<Spool[]> {
    const params = new HttpParams()
      .set('barcode', barcode); // Example barcode, replace with actual value

    return this.http.get<any>(`${this.baseUrl}spools/barcode`, { params }).pipe(map(response => response.spools));
  }
}
