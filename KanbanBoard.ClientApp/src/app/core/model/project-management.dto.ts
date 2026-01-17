export interface CreateUpdateProjectDto
{
    name:string;
    description?:string;
}

export interface ProjectDto{
    id: string;
    name: string;
    description?: string;
    createdAt: Date;
}