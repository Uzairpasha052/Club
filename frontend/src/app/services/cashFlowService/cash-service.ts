import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CashInModel } from '../../models/CashFlow/cashIn.model';
import { environment } from '../../environments/environment';
import { CashOutModel } from '../../models/CashFlow/cashOut.model';

@Injectable({
    providedIn: 'root'
})
export class CashService {

    private apiUrl = `${environment.apiUrl}/cashFlow`

    constructor(
        private http: HttpClient
    ){}

    CreateCashIn(data: CashInModel): Observable<any>{
        return this.http.post<any>(
            `${this.apiUrl}/cashin`,
            data
        )
    }

    CreateCashOut(data: CashOutModel): Observable<any>{
        return this.http.post<any>(
            `${this.apiUrl}/cashout`,
            data
        )
    }

    GetAllCashIn(year?: number, month?: number): Observable<any> {
        let params = new HttpParams();

        if (year != null) {
            params = params.set('year', year);
        }

        if (month != null) {
            params = params.set('month', month);
        }

        return this.http.get<any>(
            `${this.apiUrl}/cashin`,
            { params }
        );
    }

    GetAllCashOut(year?: number, month?: number): Observable<any> {
        let params = new HttpParams();

        if (year != null) {
            params = params.set('year', year);
        }

        if (month != null) {
            params = params.set('month', month);
        }

        return this.http.get<any>(
            `${this.apiUrl}/cashout`,
            { params }
        );
    }
}
