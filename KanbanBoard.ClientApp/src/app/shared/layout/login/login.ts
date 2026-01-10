import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '@core/api/auth.service';
import { LoginDto } from '@core/model/identity.dto';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  loginForm = this.fb.nonNullable.group({
    username: ['', [Validators.required]],
    password: ['', [Validators.required,Validators.minLength(6)]]
  });

  onLogin() {

    if(this.loginForm.invalid){
      this.loginForm.markAllAsTouched();
      return;
    }

    const payload: LoginDto = this.loginForm.getRawValue();

    if (this.loginForm.valid) {
      this.auth.login(payload).subscribe({
        next: () => {
          const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '';
          this.router.navigateByUrl(returnUrl);
        },
        error: (err) => alert('Login failed!')
      });
    }
  }
}
