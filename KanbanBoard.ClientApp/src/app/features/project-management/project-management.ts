import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { Board } from "./board/board";
import { Dashboard } from "../dashboard/dashboard";
import { Tab } from '../../shared/common/tab/tab';
import { TabPaneDirective } from '../../shared/common/tab/tab-pane.directive';
import { Project } from "./project/project";
import { ProjectCard } from 'src/app/shared/common/project-card/project-card';

@Component({
  selector: 'app-project-management',
  standalone: true,
  imports: [
    CommonModule, 
    Tab, 
    TabPaneDirective, 
    Board, 
    Dashboard, 
    Project,
    ProjectCard
  ],
  templateUrl: './project-management.html',
  styleUrl: './project-management.scss',
})
export class ProjectManagement {

  
}
