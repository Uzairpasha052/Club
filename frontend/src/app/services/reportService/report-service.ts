
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class ReportService {

    private apiUrl = `${environment.apiUrl}/reports`;

    constructor(private http: HttpClient) {}

    getPlayerRecordReport(status: string): Observable<Blob>{
        return this.http.get(
            `${this.apiUrl}/player-record?status=${status}`,
            {
                responseType: 'blob'
            }
        )
    }

    getPlayerReasonReport(): Observable<Blob> {
        return this.http.get(
            `${this.apiUrl}/player-reason`,
            {
                responseType: 'blob'
            }
        )   
    }

    getPlayerAttendanceReport(startDate: string, endDate: string): Observable<Blob> {
        return this.http.get(
            `${this.apiUrl}/attendance-record?startDate=${startDate}&endDate=${endDate}`,
            {
                responseType: 'blob'
            }
        )
    }

    getCashInReport(startDate: string, endDate: string): Observable<Blob> {
        return this.http.get(
            `${this.apiUrl}/cash-in?startDate=${startDate}&endDate=${endDate}`,
            {
                responseType: 'blob'
            }
        )
    }

    getCashOutReport( startDate: string, endDate: string): Observable<Blob> {
        return this.http.get(
            `${this.apiUrl}/cash-out?startDate=${startDate}&endDate=${endDate}`,
            {
            responseType: 'blob'
            }
        );
    }

    getCashFlowReport( startDate: string, endDate: string): Observable<Blob> {
        return this.http.get(
            `${this.apiUrl}/cash-flow?startDate=${startDate}&endDate=${endDate}`,
            {
            responseType: 'blob'
            }
        );
    }

}
