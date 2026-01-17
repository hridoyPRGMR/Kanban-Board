import { inject } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";

export abstract class BaseForm
{
    protected fb = inject(FormBuilder); 
    abstract form: FormGroup;

    getErrorMessage(controlName: string): string {
        const control = this.form.get(controlName);
        if (!control || !control.errors || !control.touched) return '';

        const errors = control.errors;
        
        if (errors['required']) return 'This field is required.';
        if (errors['minlength']) return `Minimum ${errors['minlength'].requiredLength} characters required.`;
        if(errors['maxlength'])return `Maximum ${errors['maxLength']} characters allowed.`
        if (errors['pattern']) return 'Invalid format.';
        if (errors['email']) return 'Please enter a valid email.';
        
        return 'Invalid input.';
    }

    isFieldInvalid(controlName: string): boolean {
        const control = this.form.get(controlName);
        return !!(control && control.invalid && control.touched);
    }
}