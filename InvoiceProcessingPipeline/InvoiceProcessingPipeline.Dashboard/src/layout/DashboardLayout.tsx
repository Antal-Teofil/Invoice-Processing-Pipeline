import { Outlet } from "react-router-dom";

export default function DashboardLayout() {
  return (
    <div className="relative min-h-screen overflow-hidden bg-black text-white">
      <div className="absolute inset-0 background-gradient" />

      <div className="bubble bubble-1" />
      <div className="bubble bubble-2" />
      <div className="bubble bubble-3" />
      <div className="bubble bubble-4" />

      <main className="relative z-10 mx-auto max-w-[1450px] px-8 py-10">
        <section className="rounded-3xl border border-white/10 bg-white/10 p-9 shadow-2xl backdrop-blur-xl">
          <h1 className="mb-8 text-4xl font-semibold tracking-tight">Dashboard</h1>
          <Outlet />
        </section>
      </main>
    </div>
  );
}