export interface CreateUpdateProjectDto
{
    name:string;
    description?:string;
}

export interface ProjectDto{
    name:string;
    description?:string;
    createdAt: Date
}