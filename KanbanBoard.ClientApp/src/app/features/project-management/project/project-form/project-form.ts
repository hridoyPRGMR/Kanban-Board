import { Component, inject } from '@angular/core';
import {FormBuilder, FormControl, ReactiveFormsModule, Validators} from '@angular/forms';
import { ProjectService } from '@core/api/project.service';
import { CreateUpdateProjectDto } from '@core/model/project-management.dto';

@Component({
  selector: 'app-project-form',
  imports: [ReactiveFormsModule],
  templateUrl: './project-form.html',
  styleUrl: './project-form.scss',
})
export class ProjectForm {

  private fb = inject(FormBuilder);
  private projectService = inject(ProjectService)

  projectForm = this.fb.group({
    name: ['',[Validators.required,Validators.minLength(1),Validators.maxLength(100)]],
    description: ['',[Validators.maxLength(1000)]]
  })

  onSubmit(){
    if(this.projectForm.invalid)
    {
      this.projectForm.markAllAsTouched();
      return;
    }

    const payload = this.projectForm.getRawValue as CreateUpdateProjectDto;

    this.projectService.create(payload).subscribe({
      next: (response)=>{
        this.projectForm.reset();
      },
      error: (err) => console.error('Failed to save proejct',err)
    })
  }

}
