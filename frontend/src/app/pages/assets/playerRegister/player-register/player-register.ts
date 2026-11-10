import { ChangeDetectorRef, Component, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { PlayerService } from '../../../../services/playerService/player';
import { PlayerModel } from '../../../../models/player/player.model';
// import { ConfirmationService } from 'primeng/api';

import { DatePipe } from '@angular/common';
import { TableModule } from 'primeng/table';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogModule } from 'primeng/dialog';
import { FloatLabelModule } from 'primeng/floatlabel';
import { SelectModule } from 'primeng/select';
import { FormsModule } from '@angular/forms';
import { DatePicker } from "primeng/datepicker";

import { PlayerRegisterPopUp } from '../../player-register-pop-up/player-register-pop-up';

@Component({
  selector: 'app-player-register',
  imports: [
    DatePipe,
    TableModule, InputTextModule,
    ButtonModule, IconFieldModule,
    InputIconModule, ConfirmDialogModule,
    DialogModule, FloatLabelModule,
    SelectModule, FormsModule,
    PlayerRegisterPopUp //For adding a player
    // DatePicker,
  ],
  providers: [ConfirmationService],
    schemas: [CUSTOM_ELEMENTS_SCHEMA],

  templateUrl: './player-register.html',
  styleUrl: './player-register.scss',
})
export class PlayerRegister {

  showRegister = false;
  openRegister() {
    this.showRegister = true;
  }
  closeRegister() {
    this.showRegister = false;
    this.loadUsers();
  }

  users: PlayerModel[] = [];

  constructor(
    private getPlayer: PlayerService,
    private confirmationService: ConfirmationService,
    private cdr: ChangeDetectorRef, 
    
        private messageService: MessageService

  ){}

  ngOnInit(){
    this.loadUsers();

  }

  loadUsers(): void {
    this.getPlayer.getPlayers().subscribe({
      next: (data) => {
        this.users = data;
        this.cdr.detectChanges();
        // console.log("LoadPlayer",data);
      },
      error: (error) => {
        this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: error?.error?.message || 'Unable to generate club Cash-In report.'
          });
      }
    })
  }

  confirmDelete(user: any){
    this.confirmationService.confirm({
      header: 'Delete User',
      message: `Are you sure you want to delete: ${user.stdName}?`,
      icon: 'pi pi-exclamation-triangle',

      accept: () => {
        this.deleteUser(user.id);
      }
    })
  }

  deleteUser(id: number){
    // this.users = this.users.filter(user => user.id !== id)

    this.getPlayer.deletePlayer(id).subscribe({
      next: () => {
        this.loadUsers();
      }
    });

  }

  displayEditDialog = false;

  selectedUser: PlayerModel = {
    id: 0,
    stdName: '',
    stdFName: '',
    stdAge: '',
    stdGender: '',
    stdContact: '',
    stdBelt: '',
    stdHistory: '',
    stdReason: '',
    stdFee: '',
    stdStatus: '',
    createdAt: '',
  };

  statuses = [
    { label: 'Active', value: 'Active' },
    { label: 'InActive', value: 'InActive' },
  ];

  editUser(user: PlayerModel){
    this.selectedUser = { ...user };
    this.displayEditDialog = true;
  }

  saveUser() {
    this.getPlayer.updatePlayer(this.selectedUser.id, this.selectedUser).subscribe({
      next: () => {
        this.loadUsers();
        this.displayEditDialog = false;
      },
      error: (error) => {
        this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: error?.error?.message || 'Unable to generate club Cash-In report.'
          });
      }
      
    });
  }
}
