import { Component, EventEmitter, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import Swal from 'sweetalert2';

import { Auth } from '../../../../services/authService/auth';
import { registerModel } from '../../../../models/register.model';

@Component({
  selector: 'app-register-popup',
  imports: [FormsModule, CommonModule],
  templateUrl: './register-popup.html',
  styleUrl: './register-popup.scss',
})
export class RegisterPopup {

  // @output() closee = new EventEmitter<void>();

  close = output<void>();
  
  user: registerModel ={
    fullName: '',
    email: '',
    password: '',
    role: '',
  }

  constructor(
    private auth: Auth
    
  ){}

  register(form: NgForm){

    // Stop here if required fields are empty 
    if (form.invalid) {
      form.control.markAllAsTouched(); 
      
      Swal.fire({
        icon: 'warning', 
        title: 'Missing Information', 
        text: 'Please fill in all required fields.', 
        confirmButtonText: 'OK', 
        confirmButtonColor: '#3085d6' 
        });
        return; 
    }

    this.auth.register(this.user).subscribe({
      next: (response) => {
            // console.log('User',this.user);
            // console.log('User Registered', response);

            Swal.fire({
              icon: 'success',
              title: 'User Added!',
              text: 'The User has been registered successfully.',
              confirmButtonText: 'Done',
              confirmButtonColor: '#3085d6'
            }).then(() => {
              this.close.emit();
            });
        
      },

      error: (err) => {
        // console.error(err);

        Swal.fire({
        icon: 'error',
        title: 'Registration Failed',
        text: err.error?.message || 'Something went wrong. Please try again.',
        confirmButtonText: 'OK',
        confirmButtonColor: '#d33'
      });
      }
    })

  }

  closePopup(){
    this.close.emit();
  }
}
