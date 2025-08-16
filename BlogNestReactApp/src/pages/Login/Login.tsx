import {useForm } from "react-hook-form";
import { useAuth } from "../../context/AuthContext";
import { useNavigate } from "react-router-dom";

type FormData = {
  username: string;
  password: string;
};
export default function Login() {
    const { register, handleSubmit, formState: { errors, isSubmitting } } = useForm<FormData>();
    const { login } = useAuth();
    const navigate = useNavigate();
    const onSubmit = async (data: FormData) => {
        try {
            await login(data.username, data.password);
            navigate("/posts");
        } catch (error) {
            console.error("Login failed:", error);
        }
    };

    return (<div className="max-w-md mx-auto p-6">
      <h1 className="text-2xl font-bold mb-4">Login</h1>

      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        {/* Email field */}
        <div>
          <label className="block mb-1">Email</label>
          <input
            className="w-full border rounded px-3 py-2"
            type="text"
            placeholder="user name"
            {...register("username", { required: "Email is required" })}
          />
          {errors.username && <p className="text-red-600 text-sm">{errors.username.message}</p>}
        </div>

        {/* Password field */}
        <div>
          <label className="block mb-1">Password</label>
          <input
            className="w-full border rounded px-3 py-2"
            type="password"
            placeholder="••••••••"
            {...register("password", { required: "Password is required" })}
          />
          {errors.password && <p className="text-red-600 text-sm">{errors.password.message}</p>}
        </div>

        {/* Submit */}
        <button
          type="submit"
          disabled={isSubmitting}
          className="w-full rounded px-4 py-2 border"
        >
          {isSubmitting ? "Logging in..." : "Login"}
        </button>
      </form>
    </div>
  );
}