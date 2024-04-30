import { useState, useEffect, useRef } from "react";
import { Routes, Route, Outlet, useNavigate, useParams } from "react-router-dom";

type GameInfo = {
  id: string;
  player: string;
}

function App() {
  return (
    <Routes>
      <Route path="/" element={<Layout />}>
        <Route index element={<Home />} />
        <Route path="lobby" element={<Lobby />} />
        <Route path="game/:id" element={<Game />} />
      </Route>
    </Routes>
  )
}

function Layout() {
  return (
    <div>
      <Outlet />
    </div>
  )
}

function Home() {
  return(<></>)
}

function Lobby() {
  return (
    <></>
  )
}

function Game() {
  const { id } = useParams();
  return (<></>)
}

export default App
