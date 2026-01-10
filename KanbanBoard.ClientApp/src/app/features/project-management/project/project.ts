import { Component, signal } from '@angular/core';
import { SlideOver } from '../../../shared/common/slide-over/slide-over';
import { ProjectForm } from "./project-form/project-form";

@Component({
  selector: 'app-project',
  imports: [SlideOver, ProjectForm],
  templateUrl: './project.html',
  styleUrl: './project.scss',
})
export class Project {

  protected isSlideOverOpen = signal(false);
  toggleSlideOver() {
    this.isSlideOverOpen.update(val => !val);
  }

}
