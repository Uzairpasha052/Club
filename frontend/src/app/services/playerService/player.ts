// import { Service } from '@angular/core';

// @Service()
// export class Player {}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
// import { Player } from '../models/player/player.model';
import { PlayerModel } from '../../models/player/player.model'

@Injectable({
  providedIn: 'root'
})
export class PlayerService {

  private apiUrl = `${environment.apiUrl}/player`;

  constructor(private http: HttpClient) {}

  getPlayers(): Observable<PlayerModel[]> {
    return this.http.get<PlayerModel[]>(this.apiUrl);
  }

  getPlayerById(id: number): Observable<PlayerModel> {
    return this.http.get<PlayerModel>(`${this.apiUrl}/${id}`);
  }

  createPlayer(player: PlayerModel): Observable<PlayerModel> {
    return this.http.post<PlayerModel>(this.apiUrl, player);
  }

  updatePlayer(id: number, player: PlayerModel): Observable<PlayerModel> {
    return this.http.put<PlayerModel>(`${this.apiUrl}/${id}`, player);
  }

  deletePlayer(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}