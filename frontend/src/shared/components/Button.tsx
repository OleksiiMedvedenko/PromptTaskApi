import type { ButtonHTMLAttributes, PropsWithChildren } from "react";

interface ButtonProps
  extends ButtonHTMLAttributes<HTMLButtonElement>, PropsWithChildren {}

export function Button({ children, className = "", ...props }: ButtonProps) {
  return (
    <button className={`button ${className}`} {...props}>
      {children}
    </button>
  );
}
