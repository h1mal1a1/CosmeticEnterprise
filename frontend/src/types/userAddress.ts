export type UserAddress = {
  id: number;
  idUser: number;
  recipientName: string;
  phone: string;
  country: string;
  city: string;
  street: string;
  house: string;
  apartment?: string | null;
  postalCode?: string | null;
  comment?: string | null;
  isDefault: boolean;
  createdAtUtc: string;
  updatedAtUtc: string;
};

export type CreateUserAddressRequest = {
  recipientName: string;
  phone: string;
  country: string;
  city: string;
  street: string;
  house: string;
  apartment?: string | null;
  postalCode?: string | null;
  comment?: string | null;
  isDefault: boolean;
};

export type UpdateUserAddressRequest = CreateUserAddressRequest;