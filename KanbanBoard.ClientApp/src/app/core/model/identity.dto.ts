export interface RefreshTokenDto
{
    refreshToken:string;
}

export interface TokenResponseDto
{
    accessToken:string;
    refreshToken:string;
    expiresAt:Date;
    tokenType:string;
}

export interface LoginDto
{
    username:string;
    password:string;
}

export interface RegisterUserDto
{
    email:string;
    passwordHash:string;
    username:string;
    name:string;
    phoneNumber:string;
}

export interface LoginResponseDto
{
    accessToken: string;
    refreshToken: string;
    expiresAt: Date;
    tokenType: string;
    user: UserInfoDto;
}

export interface UserInfoDto
{
    id:string;
    username:string;
    email:string;
    name:string;
    roles: string[];
}