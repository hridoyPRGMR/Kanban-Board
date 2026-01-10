import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, Validators } from '@angular/forms';
import { BaseForm } from '../../common/base-form/base-form';
import { AuthService } from '@core/api/auth.service';
import { RegisterUserDto } from '@core/model/identity.dto';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register extends BaseForm {

  private auth = inject(AuthService);
  private router = inject(Router);
  
  override form = this.fb.nonNullable.group({
    name:['',[Validators.required,Validators.minLength(3),Validators.maxLength(250)]],
    username:['',[Validators.required,Validators.minLength(3),Validators.maxLength(250)]],
    email:['',[Validators.required,Validators.email]],
    phoneNumber:['',[Validators.required,Validators.maxLength(20),Validators.pattern("^((\\+88-?)|0)?[0-9]{11}$")]],
    passwordHash:['',[Validators.required,Validators.minLength(6)]]
  });


  onSubmit(){
    if(this.form.invalid){
      this.form.markAllAsTouched();
      return;
    }

    const payload: RegisterUserDto = this.form.getRawValue();

    this.auth.register(payload).subscribe({
      next:(response)=>{
        this.router.navigate(['/login']);
      },
      error: (err)=> console.error('Registar user failed',err)
    })
  }
}
